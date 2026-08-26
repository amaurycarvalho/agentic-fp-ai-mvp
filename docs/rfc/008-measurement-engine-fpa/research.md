# Research: FPA Measurement Engine Plugin

## Overview

Researches IFPUG FPA (Function Point Analysis) standards, plugin interface design for measurement engines, and Rule Pack data model for the SpecMetrics Measurement Engine. No NEEDS CLARIFICATION items existed in the spec — this document validates technology choices and documents the IFPUG CPM 4.3 complexity matrices.

## Technology Decisions

### IFPUG Complexity Matrices (CPM 4.3)

All complexity matrices and UFP weight tables are reproduced from IFPUG CPM 4.3. These are the default counting rules used when no Rule Pack overrides them.

#### Data Functions: ILF and EIF

Complexity is determined by counting Record Element Types (RETs) and Data Element Types (DETs):

| RETs \ DETs | 1–19 | 20–50 | 51+ |
|-------------|------|-------|-----|
| 1           | Low  | Low   | Avg |
| 2–5         | Low  | Avg   | High |
| 6+          | Avg  | High  | High |

#### Transactional Functions: EI (External Inputs)

Complexity is determined by counting File Types Referenced (FTRs) and Data Element Types (DETs):

| FTRs \ DETs | 1–4 | 5–15 | 16+ |
|-------------|-----|------|-----|
| 0–1         | Low | Low  | Avg |
| 2           | Low | Avg  | High |
| 3+          | Avg | High | High |

#### Transactional Functions: EO (External Outputs)

| FTRs \ DETs | 1–5 | 6–19 | 20+ |
|-------------|-----|------|-----|
| 0–1         | Low | Low  | Avg |
| 2–3         | Low | Avg  | High |
| 4+          | Avg | High | High |

#### Transactional Functions: EQ (External Inquiries)

| FTRs \ DETs | 1–5 | 6–19 | 20+ |
|-------------|-----|------|-----|
| 0–1         | Low | Low  | Avg |
| 2–3         | Low | Avg  | High |
| 4+          | Avg | High | High |

#### Unadjusted Function Point (UFP) Weights

| Complexity | ILF | EIF | EI  | EO  | EQ  |
|------------|-----|-----|-----|-----|-----|
| Low        | 7   | 5   | 3   | 4   | 3   |
| Average    | 10  | 7   | 4   | 5   | 4   |
| High       | 15  | 10  | 6   | 7   | 6   |

### Plugin Interface Design

- **Decision**: Define a `MeasurementPlugin` Protocol that all measurement engine plugins implement — with `plugin_id()`, `supported_methodology()`, `measure(cfm, rule_pack)` methods
- **Rationale**: Existing project pattern (EventHandler protocol in Kernel). The Protocol pattern provides structural typing without inheritance coupling. Measurement plugins are discovered via Python Entry Points under `specmetrics.plugins.measurement`.
- **Alternatives considered**: ABC (abstract base class) — Protocol is the established project pattern per research.md for F06

### Rule Pack Data Model

- **Decision**: Rule Packs are YAML documents with optional overrides for complexity thresholds, UFP weights, function type exclusions, and VAF/GSC parameters
- **Rationale**: YAML is the project-standard configuration format (per constitution: ruamel.yaml). The Rule Pack model must support partial overrides — any unspecified field falls back to default IFPUG values.
- **Alternatives considered**: JSON (less readable for organizational policies), TOML (not in project stack), embedded Python (violates Rule Externalization principle IX)

### Measurement Output Format

- **Decision**: Measurement results are Pydantic models with `FPAMeasurementResult` as the top-level container, containing `MeasuredFunction` entries with full evidence trails
- **Rationale**: Pydantic v2 is the project-standard modeling library. Structured output enables F10 (Export Layer) and F11 (Publisher) to consume results without parsing knowledge of FPA internals.

### CFM Entity Mapping to FPA Function Types

- **Decision**: Map CFM entities to FPA function types using the following heuristic:
  - CFM `DataGroup` with `persistence="internal"` → ILF candidate
  - CFM `DataGroup` with `persistence="external"` → EIF candidate
  - CFM `Operation` with `direction="input"` → EI candidate
  - CFM `Operation` with `direction="output"` → EO candidate
  - CFM `Operation` with `direction="query"` → EQ candidate
- **Rationale**: The CFM defines data groups and operations with functional semantics. These are natural FPA candidates. The CFM must provide enough metadata (persistence type, operation direction, DET/RET/FTR counts) for deterministic classification.
- **Alternatives considered**: Requiring explicit FPA type declarations in the CFM (would couple CFM to FPA-specific concerns, violating Canonical Representation)

### DET/RET/FTR Derivation from CFM

- **Decision**: DET counts derive from CFM DataGroup field count (number of attributes); RET counts from CFM DataGroup sub-group count; FTR counts from CFM Operation cross-references to DataGroups
- **Rationale**: These are measurable properties of the CFM. No additional analysis or LLM involvement needed — fully deterministic.
- **Alternatives considered**: Requiring manual DET/RET/FTR annotation in CFM (would increase spec authoring burden), LLM-based guess (violates determinism)

## Integration Patterns

### Pipeline Event Flow

```
CanonicalModelBuilt (event) → CFM available in context
  → RulePackApplied (event from F09) → Rule Pack available in context
    → MeasurementCompleted (event emitted by this plugin)
```

The Measurement Engine plugin subscribes to the event after Rule Pack application. It reads the CFM and applied Rule Pack from the pipeline context.

### Relationship to Existing Specs

| Artifact | Relationship |
|----------|-------------|
| F02 (Plugin Discovery) | Measurement Engine registers via `specmetrics.plugins.measurement` entry point |
| F06 (Canonical Functional Model) | CFM is the sole semantic input — defines DataGroup, Operation, Actor entities |
| F09 (Rule Pack Engine) | Expected to provide resolved Rule Pack before measurement runs; this spec defines the Rule Pack format that F09 must produce |
| F10 (Export Layer) | Consumes `FPAMeasurementResult` structured output |
| F11 (Publisher) | Consumes `FPAMeasurementResult` structured output |

### Rule Pack Contract (Interface with F09)

Since F09 is not yet built, the Measurement Engine defines the Rule Pack interface it expects. When F09 is implemented, it must produce Rule Packs conforming to this contract. Until then, Rule Packs can be loaded directly from YAML files.

**Rule Pack capabilities**:
- Override complexity matrix thresholds (DET, RET, FTR boundaries per function type)
- Override UFP weight values per complexity level
- Exclude specific function types from counting entirely
- Define custom function type mapping rules
- Provide GSC ratings for VAF calculation
- Define exclusions by CFM element ID or name pattern
