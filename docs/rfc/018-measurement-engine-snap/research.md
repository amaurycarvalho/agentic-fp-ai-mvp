# Research: SNAP Measurement Engine Plugin

## Overview

Researches SNAP (Software Non-functional Assessment Process) methodology characteristics, plugin interface design for measurement engines, category-based assessment model, and Rule Pack data model for the SpecMetrics SNAP Measurement Engine. All ambiguity items from the clarify session have been resolved — this document validates technology choices and documents the SNAP assessment model.

## Technology Decisions

### SNAP Assessment Model

SNAP measures non-functional functional size by evaluating software characteristics across independent assessment categories. Unlike IFPUG FPA or SFP, SNAP does not count business functions — it assesses data formatting, interface presentation, operational capabilities, and technical interaction complexity.

**Decision**: Implement a category-based assessor that identifies assessment candidates from CFM semantic metadata markers (tags/annotations produced by earlier pipeline stages), classifies them into versioned categories, assigns fixed contribution values per category, and aggregates the total SNAP.

**Rationale**: The SNAP methodology evaluates non-functional characteristics that must be identified from semantic metadata — not from raw CFM entity types. Categories provide logical grouping and independent measurability.

### Assessment Candidate Identification

**Decision**: Identify assessment candidates from CFM semantic metadata markers (tags/annotations) produced by earlier pipeline stages. Each marker indicates a candidate non-functional characteristic for assessment.

**Rationale**: This was clarified during the speckit.clarify session. Unlike FPA/SFP which identify components from CFM entity types, SNAP requires semantic metadata because non-functional characteristics are not directly represented as CFM entities.

**Alternatives considered**: CFM node type filtering (non-functional characteristics are not CFM entity types), heuristic semantics (risk of non-determinism).

### Category Versioning

**Decision**: Assessment category definitions carry a SemVer string validated at engine load time. Category schema changes (new fields, changed contribution values) increment the version.

**Rationale**: Clarified during speckit.clarify. FR-015 requires versioned category definitions. SemVer provides clear compatibility semantics and aligns with the project's existing versioning conventions.

### Duplicate Merging

**Decision**: Merge duplicate assessment candidates by CFM node ID and content fingerprint (SHA-256 of `document_id`, `section_id`, `text`, `semantic_type`), reusing the Evidence Graph fingerprint mechanism.

**Rationale**: Consistent with the SFP plugin's duplicate strategy. Ensures consistent deduplication across the pipeline.

### Fixed Contribution Values

**Decision**: Use configurable default values per assessment category sourced from the licensed IFPUG SNAP specification (e.g., contribution values per category like Presentation, Data Operations, etc.). Rule Packs MAY override these defaults.

**Rationale**: The specific category contribution values are proprietary to the IFPUG SNAP methodology and must be obtained from the licensed specification. The engine supports both defaults and Rule Pack overrides.

### Plugin Interface Design

**Decision**: Implement the same `MeasurementPlugin` Protocol used by FPA and SFP plugins, with `plugin_id()`, `supported_methodology()`, `supported_function_types()`, `measure(cfm, rule_pack)` methods. The SNAP plugin registers under the `specmetrics.plugins.measurement` entry point group.

**Rationale**: Consistent with the existing measurement plugin pattern. The shared interface enables the Pipeline Engine to dispatch any methodology generically.

### Rule Pack Data Model

**Decision**: Rule Packs are YAML documents supporting: exclusion of assessment categories, exclusion of individual assessment items, redefinition of inclusion policies, and override of category contribution values. Rule Packs SHALL NOT modify the deterministic algorithm (FR-028).

**Rationale**: YAML is the project-standard configuration format. The Rule Pack model for SNAP supports both category-level and item-level exclusions, reflecting the flexible assessment model.

### Observability

**Decision**: Emit structured INFO/ERROR log messages via structlog for assessment start, completion, and failures. Emit an OpenTelemetry histogram metric for assessment duration and gauges for per-category assessment item counts.

**Rationale**: Clarified during speckit.clarify — aligns with the project's observability standards and the SFP plugin's contract.

## Integration Patterns

### Pipeline Event Flow

```
CanonicalModelBuilt (event) → CFM available in context
  → RulePackApplied (event from F09) → Rule Pack available in context
    → MeasurementCompleted (event emitted by SNAP plugin)
```

The SNAP Measurement Engine plugin subscribes to the event after Rule Pack application. It reads the CFM and applied Rule Pack from the pipeline context.

### Relationship to Existing Specs

| Artifact | Relationship |
|----------|-------------|
| F02 (Plugin Discovery) | SNAP Engine registers via `specmetrics.plugins.measurement` entry point |
| F06 (Canonical Functional Model) | CFM + semantic metadata is the sole input — SNAP identifies candidates from metadata markers |
| F09 (Rule Pack Engine) | Expected to provide resolved Rule Pack before assessment runs |
| F10 (Export Layer) | Consumes `SNAPMeasurementResult` structured output |
| F11 (Publisher) | Consumes `SNAPMeasurementResult` structured output |

### Rule Pack Contract (Interface with F09)

Since F09 is not yet built, the SNAP Measurement Engine defines the Rule Pack interface it expects. When F09 is implemented, it must produce Rule Packs conforming to this contract. Until then, Rule Packs can be loaded directly from YAML files.

**Rule Pack capabilities**:
- Exclude entire assessment categories
- Exclude individual assessment items (by CFM element ID or name pattern)
- Redefine inclusion policies (custom category-to-metadata-marker mapping)
- Override contribution values per category
