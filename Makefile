# Makefile
# Dotnet-native build/test/release + quality tooling for the agentic-fp-ai-mvp.

.PHONY: install test test-sln test-integration clean build build-images \
        lint metrics coverage coverage-check security mutation \
        install-quality-tools sonar-install sonar-up sonar-down sonar-check quality-gate help

# ---------- Variables ----------

# Single source of truth for image and release version.
VERSION ?= 1.0.0

# Coverage floor (start lower and tighten). Measured baseline (2026-08-26):
# ~72% mcp-service (real tests), ~0% agent/rag (placeholder tests) -> overall ~65%.
# Tighten as real tests land (see rag/agent orchestration changes).
COVERAGE_THRESHOLD ?= 60

# Root solution (contains the 3 MVP services: agent, mcp, rag).
SOLUTION := agentic-fp-ai-mvp.sln

# Service directories (for metrics and per-service tooling).
SERVICES := agent-service mcp-service rag-service

# Service container images produced by `build-images`.
IMAGES := agent-service mcp-service rag-service

# Per-service test projects (used by sonar-check and mutation), one per service.
TEST_PROJECTS := services/agent-service/tests/AgentService.Api.Tests/AgentService.Api.Tests.csproj \
                 services/mcp-service/tests/McpService.Api.Tests/McpService.Api.Tests.csproj \
                 services/rag-service/tests/RagService.Api.Tests/RagService.Api.Tests.csproj

# Output colors.
GREEN := \033[0;32m
RED := \033[0;31m
YELLOW := \033[0;33m
NC := \033[0m

# ---------- Main commands ----------

install:
	@echo "$(GREEN)📦 Restoring .NET dependencies...$(NC)"
	@dotnet restore "$(SOLUTION)" || exit 1
	@echo "$(GREEN)✅ Restore complete$(NC)"

test:
	@echo "$(GREEN)🧪 Running unit tests + coverage (excluding live-stack MCP integration)...$(NC)"
	@$(MAKE) test-sln SLN="$(SOLUTION)"
	@echo "$(GREEN)✅ Tests passed$(NC)"

test-sln:
	@name="$$(basename "$(SLN)" .sln)"; \
	echo "  -> dotnet test $(SLN)"; \
	dotnet test "$(SLN)" \
		--filter "Category!=Mcp.Integration" \
		--results-directory "TestResults/$$name" \
		--collect:"XPlat Code Coverage" \
		--settings CodeCoverage.runsettings \
		--logger "trx;LogFileName=results.trx" || exit 1

test-integration:
	@echo "$(GREEN)🔗 Running MCP cross-service integration tests (requires a running stack via MCP_BASE_URL)...$(NC)"
	@dotnet test services/mcp-service/tests/McpService.Api.Tests/McpService.Api.Tests.csproj \
		--filter "Category=Mcp.Integration" \
		--results-directory "TestResults/McpService.Api.Tests" \
		--collect:"XPlat Code Coverage" \
		--settings CodeCoverage.runsettings \
		--logger "trx;LogFileName=results.trx" || exit 1
	@echo "$(GREEN)✅ Integration tests passed$(NC)"

clean:
	@echo "$(YELLOW)🧹 Cleaning build artifacts...$(NC)"
	@dotnet clean "$(SOLUTION)" || exit 1
	@find . -type d \( -name bin -o -name obj \) -not -path './.git/*' -prune -exec rm -rf {} + 2>/dev/null || true
	@rm -rf TestResults images
	@echo "$(GREEN)✅ Clean complete$(NC)"

build:
	@echo "$(GREEN)📦 Building all solutions (Release)...$(NC)"
	@dotnet build "$(SOLUTION)" -c Release || exit 1
	@echo "$(GREEN)✅ Build complete$(NC)"

build-images:
	@echo "$(GREEN)🐳 Building service images (VERSION=$(VERSION))...$(NC)"
	@$(MAKE) image-agent-service
	@$(MAKE) image-mcp-service
	@$(MAKE) image-rag-service
	@echo "$(GREEN)✅ Images built and tagged$(NC)"

# Per-service image builds.
# All Dockerfiles COPY the repo root (./services/<svc>/src/...) so the build
# context is the repository root.
image-agent-service:
	docker build -t agent-service:$(VERSION) -t agent-service:latest \
		-f services/agent-service/Dockerfile .

image-mcp-service:
	docker build -t mcp-service:$(VERSION) -t mcp-service:latest \
		-f services/mcp-service/Dockerfile .

image-rag-service:
	docker build -t rag-service:$(VERSION) -t rag-service:latest \
		-f services/rag-service/Dockerfile .

# ---------- Quality targets ----------

# SonarQube local analysis (self-hosted server; see `make sonar-check`).
SONAR_HOST_URL ?= http://localhost:9000
SONAR_TOKEN ?=
SONAR_PROJECT_KEY_PREFIX ?= agentic-fp-ai-
SONAR_COMPOSE_FILE ?= sonarqube/docker-compose.yml

lint:
	@echo "$(GREEN)🔍 Lint (dotnet format --verify-no-changes)...$(NC)"
	@dotnet format "$(SOLUTION)" --verify-no-changes || exit 1
	@echo "$(GREEN)✅ Lint passed$(NC)"

metrics:
	@echo "$(GREEN)📊 Code metrics (Lines of Code)...$(NC)"
	@for svc in $(SERVICES); do \
		count="$$(find services/$$svc/src -name '*.cs' -not -path '*/obj/*' -not -path '*/bin/*' -exec cat {} + 2>/dev/null | wc -l)"; \
		echo "  $$svc: $$count LOC"; \
	done
	@echo "  (complexity / code smells / sqale / maintainability: provided by SonarCloud)"
	@echo "$(GREEN)✅ Metrics complete$(NC)"

coverage: test coverage-check

coverage-check:
	@echo "$(GREEN)📊 Checking coverage against threshold (>= $(COVERAGE_THRESHOLD)%)...$(NC)"
	@python3 scripts/coverage_check.py "$(COVERAGE_THRESHOLD)"

security:
	@echo "$(GREEN)🔒 Security scan (dependencies + SAST)...$(NC)"
	@for proj in $(TEST_PROJECTS); do \
		dir="$$(dirname "$$proj")"; \
		echo "  -> $$proj --vulnerable"; \
		out="$$(cd "$$dir" && dotnet list package --vulnerable 2>&1)"; \
		echo "$$out"; \
		if echo "$$out" | grep -qi "vulnerab" && ! echo "$$out" | grep -qi "no vulnerable packages"; then \
			echo "$(RED)❌ Vulnerable packages found$(NC)"; exit 1; \
		fi; \
		echo "  -> $$proj --deprecated"; \
		(cd "$$dir" && dotnet list package --deprecated); \
		echo "  -> $$proj --outdated"; \
		(cd "$$dir" && dotnet list package --outdated); \
	done
	@echo "  -> semgrep (C# source)..."
	@if command -v semgrep >/dev/null 2>&1; then \
		semgrep ci --oss-only --quiet --config auto --include '*.cs' || exit 1; \
	else \
		echo "$(YELLOW)⚠️ semgrep not found - skipping SAST scan (run 'make install-quality-tools')$(NC)"; \
	fi
	@echo "$(GREEN)✅ Security scan complete$(NC)"

mutation:
	@echo "$(GREEN)🧬 Running mutation tests (Stryker.NET, manual)...$(NC)"
	@for tp in services/*/tests/*.Api.Tests; do \
		echo "  -> dotnet-stryker in $$tp"; \
		(cd "$$tp" && dotnet-stryker --test-runner mtp) || exit 1; \
	done
	@echo "$(GREEN)✅ Mutation tests complete$(NC)"

install-quality-tools:
	@echo "$(GREEN)🔧 Installing quality tools...$(NC)"
	@dotnet tool install --global dotnet-stryker || true
	@if ! command -v semgrep >/dev/null 2>&1; then python3 -m pip install --user semgrep; fi
	@echo "$(GREEN)✅ Quality tools installed (dotnet-stryker + semgrep; dotnet-format bundled)$(NC)"

sonar-install:
	@echo "$(GREEN)📡 Installing dotnet-sonarscanner (global tool)...$(NC)"
	@dotnet tool install --global dotnet-sonarscanner
	@echo "$(GREEN)✅ dotnet-sonarscanner installed$(NC)"

sonar-up:
	@echo "$(GREEN)🐳 Starting local SonarQube stack ($(SONAR_COMPOSE_FILE))...$(NC)"
	@docker-compose -f $(SONAR_COMPOSE_FILE) up -d
	@echo "$(GREEN)✅ SonarQube ready at $(SONAR_HOST_URL)$(NC)"
	@echo "  - First login: admin / admin  (change the password on first login!)"
	@echo "  - Generate an analysis token: My Account -> Security -> Tokens (as admin, projects are auto-created)"
	@echo "  - Then run: SONAR_TOKEN=<token> make sonar-check"
	@echo "  - Per-service project keys:"
	@for svc in $(SERVICES); do \
		echo "      $(SONAR_PROJECT_KEY_PREFIX)$$svc"; \
	done

sonar-down:
	@echo "$(GREEN)🐳 Stopping local SonarQube stack (volumes preserved)...$(NC)"
	@docker-compose -f $(SONAR_COMPOSE_FILE) down
	@echo "$(GREEN)✅ SonarQube stopped (data persists; full reset: docker compose -f $(SONAR_COMPOSE_FILE) down -v)$(NC)"

sonar-check:
	@test -n "$(SONAR_TOKEN)" || { echo "$(RED)❌ SONAR_TOKEN is required - export it (e.g. SONAR_TOKEN=xxx make sonar-check)$(NC)"; exit 1; }
	@for proj in $(TEST_PROJECTS); do \
		svc="$$(echo "$$proj" | sed -E 's#services/([^/]+)/.*#\1#')"; \
		key="$(SONAR_PROJECT_KEY_PREFIX)$$svc"; \
		name="$$(basename "$$proj" .csproj)"; \
		echo "$(GREEN)📡 SonarQube analysis: $$key -> $(SONAR_HOST_URL)$(NC)"; \
		( \
			trap 'dotnet sonarscanner end /d:sonar.token="$(SONAR_TOKEN)"' EXIT; \
			dotnet sonarscanner begin /k:"$$key" \
				/d:sonar.host.url="$(SONAR_HOST_URL)" \
				/d:sonar.token="$(SONAR_TOKEN)" \
				/v:"$(VERSION)" \
				/d:sonar.cs.cobertura.reportsPaths="TestResults/$$name/**/coverage.cobertura.xml" \
				/d:sonar.coverage.exclusions="**/*Tests/**" || exit 1; \
			$(MAKE) test-sln SLN="$$proj" || exit 1; \
		); \
		status=$$?; \
		if [ $$status -ne 0 ]; then exit $$status; fi; \
	done
	@echo "$(GREEN)✅ SonarQube analysis complete$(NC)"

# ---------- Quality gate ----------
# Lint + test(coverage) + coverage-check + metrics + security.
# Mutation (Stryker.NET) is excluded and run manually via `make mutation`.
# SonarCloud analysis + PR decoration run as a CI step.

quality-gate:
	@echo "$(GREEN)🚀 Running quality gate...$(NC)"
	@$(MAKE) install
	@$(MAKE) lint
	@$(MAKE) test
	@$(MAKE) coverage-check
	@$(MAKE) metrics
	@$(MAKE) security
	@echo "$(GREEN)🎉 All quality checks passed!$(NC)"

# ---------- Help ----------

help:
	@echo "$(GREEN)📋 Available commands:$(NC)"
	@echo ""
	@echo "  make install          - Restore .NET dependencies (root solution)"
	@echo "  make test             - Run unit tests + collect coverage"
	@echo "  make test-integration - Run MCP cross-service integration tests (requires MCP_BASE_URL)"
	@echo "  make clean            - Clean build artifacts and outputs"
	@echo "  make build            - Compile the root solution in Release"
	@echo "  make build-images VERSION=x.x.x - Build and tag the 3 service images"
	@echo "  make lint             - Verify formatting/analyzers (dotnet format --verify-no-changes)"
	@echo "  make metrics          - Report Lines of Code per service"
	@echo "  make coverage         - Run tests and check coverage threshold"
	@echo "  make coverage-check   - Check coverage against COVERAGE_THRESHOLD (default 80)"
	@echo "  make security         - Check package vulnerabilities/deprecated/outdated + Semgrep SAST"
	@echo "  make mutation         - Run Stryker.NET mutation tests (manual, not in CI)"
	@echo "  make install-quality-tools - Install dotnet-stryker + semgrep"
	@echo "  make sonar-install    - Install the dotnet-sonarscanner global tool"
	@echo "  make sonar-up         - Start the local SonarQube stack (docker compose up --wait)"
	@echo "  make sonar-down       - Stop the local SonarQube stack (volumes preserved)"
	@echo "  make sonar-check      - Run per-service SonarQube analysis (needs SONAR_TOKEN; SONAR_HOST_URL default http://localhost:9000)"
	@echo "  make quality-gate     - Run the quality gate (lint + test + coverage + metrics + security)"
	@echo "  make help             - Show this help message"
	@echo ""
	@echo "$(YELLOW)Examples:$(NC)"
	@echo "  make build-images VERSION=2.0.0"
	@echo "  MCP_BASE_URL=http://localhost:8082 make test-integration"
	@echo "  COVERAGE_THRESHOLD=90 make coverage-check"
	@echo "  SONAR_TOKEN=xxx SONAR_HOST_URL=http://localhost:9000 make sonar-check"
