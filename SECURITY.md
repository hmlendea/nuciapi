# Security Policy

This policy defines how to report security vulnerabilities for NuciAPI, establishes coordinated disclosure expectations, and documents which release channels are currently maintained for security updates.

## 📑 Table of Contents

- [Table of Contents](#-table-of-contents)
- [Supported Versions](#-supported-versions)
- [Reporting a Vulnerability](#-reporting-a-vulnerability)
- [Scope](#-scope)
- [Disclosure Policy](#-disclosure-policy)

## 🛡️ Supported Versions

Use this table to indicate which project versions currently receive security maintenance.

| Version | Distribution Channel | Supported |
|---------|--------------------|-----------|
| Latest version | NuGet (nuget.org) | ✅ |
| Latest version | GitHub repository source distribution | ✅ |
| Latest version | Unofficial package mirrors | ❌ |
| Latest version | Repackaged third-party binaries | ❌ |
| Latest version | Unofficial third-party distribution channels | ❌ |
| Preceding versions | Any distribution channel | ❌ |

## 🚨 Reporting a Vulnerability

Please do not disclose suspected vulnerabilities publicly before maintainers have had an opportunity to validate and remediate them.

To report a vulnerability:
- [GitHub Security Advisories](https://github.com/hmlendea/nuciapi/security/advisories)
- Contact the maintainers directly

## 📌 Scope

The subsequent report categories are in scope for this repository:
- Vulnerabilities in NuciAPI source code and published package artefacts
- Bypasses or weaknesses affecting HMAC signing or HMAC validation behaviour

The subsequent categories are out of scope unless explicitly stated to the contrary:
- General support inquiries or usage misconfiguration without a reproducible security impact
- Security issues in third-party systems, infrastructure, or services that are external to this repository

## 📢 Disclosure Policy

This project follows coordinated disclosure:
1. Vulnerabilities are investigated privately.
2. A remediation plan is prepared and validated.
3. Public disclosure is published after a fix, mitigation, or agreed risk decision is available.
4. Credit is attributed in accordance with reporter preference and project policy.
