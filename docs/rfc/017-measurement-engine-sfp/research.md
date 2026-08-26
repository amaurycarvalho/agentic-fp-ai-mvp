# Research: SFP Measurement Engine Plugin

## Overview

Researches Simple Function Points (SFP) methodology characteristics, plugin interface design for measurement engines, and Rule Pack data model for the SpecMetrics SFP Measurement Engine. All ambiguity items from the clarify session have been resolved — this document validates technology choices and documents the simplified SFP measurement model.

## Technology Decisions

### SFP Measurement Model

SFP simplifies function point counting by reducing the measurable component types to two: Functional Processes and Logical Functions. Unlike IFPUG FPA, there is no complexity classification (Low/Average/High), no DET/RET/FTR counting, and no transaction subclasses.

**Decision**: Implement a two-component counter that identifies Functional Processes and Logical Functions from CFM node types and semantic metadata, assigns fixed contribution values, and aggregates the total SFP.

**Rationale**: The SFP methodology intentionally reduces measurement complexity. The engine must be equally simple — no complexity matrices, no weight tables, no VAF/GSC adjustments.

### Component Identification Strategy

**Decision**: Identify Functional Processes from CFM nodes where `node_type == "elementary_process"` or equivalent semantic marker, and Logical Functions from CFM Data Group nodes where the node represents user-recognizable persistent business information.

**Rationale**: This was clarified during the speckit.clarify session — CFM node type/attribute matching is the most deterministic approach consistent with the no-LLM constraint.

**Alternatives considered**: Heuristic pattern matching (risk of non-determinism), plugin-provided identification rules (adds complexity without benefit for MVP).

### Duplicate Merging Strategy

**Decision**: Merge duplicate components by CFM node ID AND content fingerprint (SHA-256 of `document_id`, `section_id`, `text`, `semantic_type`), reusing the Evidence Graph fingerprint mechanism.

**Rationale**: Clarified during speckit.clarify — ensures consistent deduplication across the pipeline by leveraging the existing fingerprint infrastructure from F06 (Evidence Graph).

### Fixed Contribution Values

**Decision**: Use configurable default values sourced from the licensed IFPUG SFP specification (e.g., 4.6 SFP per Functional Process, 7.1 SFP per Logical Function). Rule Packs MAY override these defaults.

**Rationale**: FR-019 and FR-020 require fixed values per component type. The specific values are proprietary to the IFPUG SFP methodology and must be obtained from the licensed specification. The engine must support both defaults and Rule Pack overrides.

**Alternatives considered**: Hardcoding values (would violate licensing), omitting defaults entirely (would require Rule Pack for every measurement).

### Plugin Interface Design

**Decision**: Implement the same `MeasurementPlugin` Protocol used by the FPA plugin, with `plugin_id()`, `supported_methodology()`, `measure(cfm, rule_pack)` methods. The SFP plugin registers under the `specmetrics.plugins.measurement` entry point group.

**Rationale**: Consistent with the FPA plugin and the existing Kernel Protocol pattern. The shared interface enables the Pipeline Engine to dispatch any measurement methodology generically.

**Alternatives considered**: Custom SFP-specific interface (would break pipeline genericity).

### Rule Pack Data Model

**Decision**: Rule Packs are YAML documents supporting: exclusion of Functional Processes or Logical Functions, redefinition of inclusion criteria, and override of fixed contribution values. Rule Packs SHALL NOT modify the deterministic algorithm (FR-031).

**Rationale**: YAML is the project-standard configuration format. The Rule Pack model for SFP is simpler than FPA's — no complexity overrides, no weight tables, no GSC/VAF parameters.

### Observability

**Decision**: Emit structured INFO/ERROR log messages via structlog for measurement start, completion, and failures. Emit an OpenTelemetry histogram metric for measurement duration and gauges for component counts (Logical Functions, Functional Processes).

**Rationale**: Clarified during speckit.clarify — aligns with the project's observability standards and enables operational monitoring without excessive overhead.

## Integration Patterns

### Pipeline Event Flow

```
CanonicalModelBuilt (event) → CFM available in context
  → RulePackApplied (event from F09) → Rule Pack available in context
    → MeasurementCompleted (event emitted by SFP plugin)
```

The SFP Measurement Engine plugin subscribes to the event after Rule Pack application. It reads the CFM and applied Rule Pack from the pipeline context.

### Relationship to Existing Specs

| Artifact | Relationship |
|----------|-------------|
| F02 (Plugin Discovery) | SFP Engine registers via `specmetrics.plugins.measurement` entry point |
| F06 (Canonical Functional Model) | CFM is the sole semantic input — SFP identifies Data Groups as Logical Functions and Elementary Processes as Functional Processes |
| F09 (Rule Pack Engine) | Expected to provide resolved Rule Pack before measurement runs |
| F10 (Export Layer) | Consumes `SFPMeasurementResult` structured output |
| F11 (Publisher) | Consumes `SFPMeasurementResult` structured output |

### Rule Pack Contract (Interface with F09)

Since F09 is not yet built, the SFP Measurement Engine defines the Rule Pack interface it expects. When F09 is implemented, it must produce Rule Packs conforming to this contract. Until then, Rule Packs can be loaded directly from YAML files.

**Rule Pack capabilities**:
- Exclude specific Functional Processes (by CFM element ID or name pattern)
- Exclude specific Logical Functions (by CFM element ID or name pattern)
- Redefine inclusion criteria (custom node type/attribute matching rules)
- Override fixed contribution values per component type
