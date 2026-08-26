#!/usr/bin/env python3
"""Check .NET test coverage (Cobertura XML) against a threshold.

Reads every coverage.cobertura.xml under TestResults/, aggregates covered and
valid lines, and exits non-zero when the overall coverage is below the given
threshold.

Usage:
    python3 scripts/coverage_check.py <threshold_percent> [coverage_root]

Requires only the Python standard library.
"""

import sys
import glob
import xml.etree.ElementTree as ET


def main() -> int:
    if len(sys.argv) < 2:
        print("usage: coverage_check.py <threshold_percent> [coverage_root]",
              file=sys.stderr)
        return 2

    try:
        threshold = float(sys.argv[1])
    except ValueError:
        print(f"invalid threshold: {sys.argv[1]!r}", file=sys.stderr)
        return 2

    root = sys.argv[2] if len(sys.argv) > 2 else "TestResults"
    pattern = f"{root}/**/coverage.cobertura.xml"
    files = [
        path for path in sorted(glob.glob(pattern, recursive=True))
        # Skip transient/internal directories (e.g. "_recruta-zero_*") that
        # are not produced by `make test`.
        if not any(part.startswith("_") for part in path.split("/"))
    ]

    if not files:
        print(f"no coverage reports found under {root!r} "
              f"(pattern: {pattern!r})", file=sys.stderr)
        return 1

    total_covered = 0
    total_valid = 0
    for path in files:
        try:
            tree = ET.parse(path)
        except ET.ParseError as exc:
            print(f"failed to parse {path}: {exc}", file=sys.stderr)
            return 1

        coverage = tree.getroot()
        lines_covered = int(coverage.get("lines-covered") or 0)
        lines_valid = int(coverage.get("lines-valid") or 0)
        total_covered += lines_covered
        total_valid += lines_valid
        print(f"  {path}: {lines_covered}/{lines_valid} lines covered")

    if total_valid <= 0:
        print("no coverable lines found in the reports", file=sys.stderr)
        return 1

    coverage_pct = (total_covered / total_valid) * 100.0
    print(f"\n  Overall coverage: {coverage_pct:.2f}% "
          f"({total_covered}/{total_valid} lines)")

    if coverage_pct + 1e-9 < threshold:
        print(f"  ❌ Coverage below threshold ({threshold:.2f}%)",
              file=sys.stderr)
        return 1

    print(f"  ✅ Coverage >= threshold ({threshold:.2f}%)")
    return 0


if __name__ == "__main__":
    sys.exit(main())
