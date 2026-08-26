# Feature Specification: Measurement Engine Plugin — SNAP

**Feature Branch**: `018-measurement-engine-snap`

**Created**: 2026-07-16

**Status**: Draft

**Input**: User description: "F18"

## Clarifications

### Session 2026-07-16

- Q: How should the engine identify SNAP assessment candidates from the CFM? → A: CFM semantic metadata markers produced by earlier pipeline stages.
- Q: What defines a "medium-sized CFM" for the SC-003 five-second performance target? → A: ≤500 assessment candidates.
- Q: How should assessment category definitions be versioned (FR-015)? → A: Semantic versioning (SemVer) on category schema, validated at engine load time.
- Q: What observability signals should the SNAP measurement engine emit? → A: Structured logging + OpenTelemetry metrics (duration histogram, per-category count gauges).
- Q: What tolerance defines "approximately linear" scaling for SC-007? → A: ≤15% deviation per doubling of assessment candidate count.

---

# Overview

The SNAP (Software Non-functional Assessment Process) Measurement Engine is a deterministic SpecMetrics plugin responsible for measuring the **non-functional functional size** of a software application.

Unlike Function Point Analysis (FPA) and Simple Function Points (SFP), which quantify business functionality, SNAP quantifies software characteristics that influence implementation effort without increasing functional size, such as data formatting, interface presentation, operational capabilities and technical interaction complexity.

The plugin consumes the Canonical Functional Model (CFM), enriched with semantic metadata produced by previous pipeline stages, and produces an explainable, repeatable SNAP assessment.

The engine performs no semantic inference and executes deterministically.

---

# Objectives

The Measurement Engine SHALL:

- measure non-functional functional size;
- complement FPA or SFP measurements;
- preserve deterministic execution;
- provide complete explainability;
- integrate with Rule Packs;
- execute as a Kernel Measurement Plugin.

---

# User Scenarios & Testing

## User Story 1 — Automatic SNAP Measurement (Priority: P1)

A software estimator executes:

```text
specmetrics measure --method snap
```

The engine analyzes the Canonical Functional Model and computes the total SNAP size together with a breakdown by assessment category.

### Acceptance

- measurement completes automatically;
- every assessed item appears in the report;
- repeated executions produce identical results.

---

## User Story 2 — Explainable Assessment (Priority: P1)

A reviewer wants to understand why a particular interface or operational capability contributed to the final SNAP result.

The engine exposes:

- originating CFM elements;
- assessment category;
- applied measurement rules;
- contribution.

---

## User Story 3 — Organizational Policies (Priority: P2)

An organization defines a Rule Pack excluding internal administrative interfaces from SNAP assessment.

The engine applies the exclusions while preserving traceability.

---

## User Story 4 — Pipeline Integration (Priority: P2)

The SNAP plugin is automatically discovered and executed after semantic normalization and Rule Pack processing.

---

# Edge Cases

- Empty Canonical Functional Model
- Missing presentation metadata
- Missing operational metadata
- Duplicate assessment candidates
- Unsupported interaction types
- Invalid Rule Packs
- Corrupted plugin metadata
- Extremely large models

---

# Constitution Check

## Principle IV — Deterministic Execution

Satisfied.

Assessment is entirely rule-based.

---

## Principle VI — Explainability

Satisfied.

Every assessed element preserves evidence references.

---

## Principle VII — Canonical Representation

Satisfied.

Only Canonical Functional Model elements are consumed.

---

## Principle VIII — Plugin Architecture

Satisfied.

Plugin discovery occurs through the Measurement Plugin registry.

---

## Principle IX — Rule Externalization

Satisfied.

Assessment policies remain external to the engine.

---

# Functional Requirements

## General

### FR-001

The Measurement Engine SHALL consume only the Canonical Functional Model and its associated semantic metadata.

---

### FR-002

The engine SHALL execute without probabilistic or AI-based decisions.

---

### FR-003

The engine SHALL produce deterministic assessments.

---

### FR-004

Every assessed item SHALL preserve evidence references.

---

### FR-005

The engine SHALL support Rule Packs.

---

### FR-006

The engine SHALL produce machine-readable output.

---

### FR-007

The engine SHALL execute as a discoverable Measurement Plugin.

---

### FR-008

The engine SHALL support incremental execution.

---

### FR-009

The engine SHALL support event-driven pipeline execution.

---

### FR-010

Warnings SHALL NOT interrupt assessment.

---

# Assessment Model

The SNAP plugin evaluates software characteristics that are outside the scope of functional sizing.

Assessment is organized into independent categories representing distinct classes of non-functional user requirements.

Each category contributes independently to the overall SNAP result.

---

# Assessment Categories

### FR-011

The engine SHALL organize assessed items by assessment category.

---

### FR-012

Each assessed item SHALL belong to exactly one category.

---

### FR-013

Categories SHALL be independently measurable.

---

### FR-014

Assessment categories SHALL remain extensible through Rule Packs.

---

### FR-015

Category definitions SHALL be versioned using semantic versioning (SemVer) on the category schema, validated at engine load time.

---

# Assessment Rules

### FR-016

The engine SHALL identify assessment candidates from CFM semantic metadata markers (tags/annotations produced by earlier pipeline stages).

---

### FR-017

Duplicate candidates SHALL be merged.

---

### FR-018

Assessment SHALL ignore implementation technologies.

---

### FR-019

Assessment SHALL be based on user-visible software characteristics.

---

### FR-020

Each assessed item SHALL contribute independently to the total SNAP result.

---

### FR-021

Assessment SHALL be reproducible.

---

### FR-022

Assessment SHALL preserve category-specific evidence.

---

### FR-023

The engine SHALL report excluded assessment candidates.

---

### FR-024

The engine SHALL report unresolved assessment candidates as warnings.

---

# Rule Packs

### FR-025

Rule Packs MAY exclude assessment categories.

---

### FR-026

Rule Packs MAY exclude individual assessment items.

---

### FR-027

Rule Packs MAY redefine inclusion policies.

---

### FR-028

Rule Packs SHALL NOT alter deterministic execution.

---

### FR-029

All Rule Pack adjustments SHALL be reported.

---

# Explainability

### FR-030

Every assessed item SHALL expose:

- originating CFM element;
- assessment category;
- applied Rule Pack;
- contribution;
- evidence references.

---

### FR-031

Evidence SHALL be immutable.

---

### FR-032

Evidence SHALL survive export.

---

# Pipeline

### FR-033

Pipeline execution order SHALL be:

```text
Semantic Extraction

↓

Canonical Functional Model

↓

Rule Pack Engine

↓

SNAP Measurement Engine

↓

Export Layer
```

---

### FR-034

The plugin SHALL emit Measurement Events.

---

### FR-035

The plugin SHALL support asynchronous execution.

---

### FR-036

The plugin SHALL support incremental recomputation.

---

### FR-037

Only modified assessment candidates SHALL be recomputed.

---

# Observability

### FR-038

The engine SHALL emit structured INFO/ERROR log messages for assessment start, completion, and failures.

---

### FR-039

The engine SHALL emit an OpenTelemetry histogram metric for assessment duration and gauges for per-category assessment item counts.

---

# Measurement Result

The output SHALL include:

```text
Measurement Method

Version

Measurement Timestamp

Assessment Categories

Assessment Item Count

Total SNAP

Applied Rule Packs

Evidence References

Warnings

Execution Statistics
```

---

# Key Entities

## SNAP Measurement Result

Complete assessment output.

---

## Assessment Category

Logical grouping of related non-functional assessment items.

---

## Assessment Item

A measurable software characteristic contributing to the SNAP result.

---

## Measured Component

A normalized representation of an assessment item.

---

## Measurement Evidence

Traceability information linking the assessment to originating CFM elements.

---

## Rule Pack

External policy controlling inclusion and exclusion of assessment items.

---

# Plugin Interfaces

```text
MeasurementPlugin

measure()

validate()

describe()

supported_methods()

version()
```

---

# Output Example

```json
{
  "method": "SNAP",
  "categories": [
    {
      "name": "Presentation",
      "value": 42
    },
    {
      "name": "Data Operations",
      "value": 28
    }
  ],
  "total_snap": 70,
  "warnings": [],
  "rule_pack": "default",
  "explainability": []
}
```

---

# Success Criteria

### SC-001

Repeated executions SHALL produce byte-identical results.

### SC-002

Every assessed item SHALL contain evidence references.

### SC-003

Medium-sized CFMs (≤500 assessment candidates) SHALL be processed within five seconds.

### SC-004

Incremental execution SHALL only reassess modified elements.

### SC-005

Plugin discovery SHALL require no manual configuration.

### SC-006

Invalid Rule Packs SHALL generate warnings without aborting execution.

### SC-007

Assessment SHALL scale approximately linearly with the number of assessment candidates (≤15% deviation per doubling of candidate count).

---

# Non-Goals

The plugin does **not**:

- perform semantic extraction;
- parse source code;
- invoke LLMs;
- estimate development effort;
- estimate project cost;
- replace FPA or SFP measurements;
- generate documentation.

---

# Assumptions

- The Canonical Functional Model has already been validated.
- Semantic metadata required for non-functional assessment has been produced by previous pipeline stages.
- Rule Packs are resolved before measurement.
- Export plugins consume Measurement Results independently of the assessment methodology.
- SNAP complements, rather than replaces, functional sizing methods such as FPA and SFP.
- Future versions of SpecMetrics may support additional non-functional sizing methodologies through separate measurement plugins.

> **Note:** This specification intentionally defines the **plugin architecture, contracts, deterministic behavior, explainability model, and integration requirements** for a SNAP measurement engine. It does not reproduce the proprietary counting rules, scoring tables, or detailed assessment procedures defined in the official SNAP methodology. Implementations intended to claim conformance with SNAP should use the licensed specification published by IFPUG.
