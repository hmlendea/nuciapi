# Privacy and Personal Data

**Information reviewed:** 2026-10-10

## 📑 Table of Contents

- [What This Document Covers](#what-this-document-covers)
- [Self-Hosted Deployments](#self-hosted-deployments)
- [Data We Handle](#data-we-handle)
  - [Data Provided to the Application](#data-provided-to-the-application)
  - [Data Generated or Collected by the Application](#data-generated-or-collected-by-the-application)
  - [Data Received from Integrations](#data-received-from-integrations)
- [Processing and Use](#processing-and-use)
- [Storage, Retention, and Deletion](#storage-retention-and-deletion)
- [External Processing and Integrations](#external-processing-and-integrations)
- [Document Changes](#document-changes)
- [Contact](#contact)

## 🔎 What This Document Covers

This document describes how NuciAPI at https://github.com/hmlendea/nuciapi handles personal data. It covers the library behaviour and verified integrations described below. Where the software is self-hosted, the instance operator may have separate responsibilities described below.

## 🏠 Self-Hosted Deployments

NuciAPI is a .NET library consumed by applications. It does not operate as a standalone service. The library itself has no configuration, no telemetry, no network calls, and no persistent storage. Instance operators (application developers using NuciAPI) control all data handling in their applications, including any request/response payloads that may contain personal data, local storage, logs, backups, access controls, retention, and request handling. The library does not send any data to project maintainers or external services.

## 📥 Data We Handle

### Data Provided to the Application

No personal data is requested or required by the library itself. Applications using NuciAPI may pass request/response objects that contain personal data as part of their business logic; this data is provided by the application, not the library.

### Data Generated or Collected by the Application

No personal data is generated or collected automatically by the library. The library performs HMAC signing and validation on object graphs provided by the caller; it does not log, persist, or transmit any data.

### Data Received from Integrations

No personal data is received from integrations or third parties. The library has a single compile-time dependency on `NuciSecurity.HMAC` for cryptographic operations; this dependency performs no network calls and collects no data.

## 🧭 Processing and Use

The library processes the data described above for these verified functions:
- HMAC signing of request/response objects — object graph provided by caller (may include personal data if the application includes it)
- HMAC validation of request/response objects — object graph and token provided by caller

## 🗄️ Storage, Retention, and Deletion

The library has no storage, retention, or deletion behaviour. It holds no state between operations. All data exists only in the caller's object instances for the duration of the method call. Instance operators control storage, retention, and deletion of any data in their applications.

## 🔗 External Processing and Integrations

The application has no built-in external data transfer. The only integration is the compile-time dependency on `NuciSecurity.HMAC` (v4.1.3), which performs local cryptographic operations and makes no network calls.

| Service or integration | Purpose | Data involved | Configuration or documentation |
|-----------------------|---------|---------------|--------------------------------|
| NuciSecurity.HMAC | Cryptographic HMAC signing/validation | Object graph properties (caller-controlled) | https://github.com/hmlendea/nucisecurity.hmac |

## 🔄 Document Changes

Update this document when library data flows, storage, integrations, or deployment responsibilities change. The current version is published at https://github.com/hmlendea/nuciapi/blob/main/PRIVACY.md.

## 📬 Contact

For questions about library data handling, contact the project maintainers via GitHub issues at https://github.com/hmlendea/nuciapi/issues. For a self-hosted application using NuciAPI, contact the application operator. Do not send passwords, access tokens, or other secrets.