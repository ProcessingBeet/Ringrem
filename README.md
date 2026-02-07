# Call Reminder

## About

We often forget to call friends or family.  
Not calendar-style calls at fixed dates, but simple “call from time to time” check-ins.

This project is an application that **reminds you to call people**.

The idea is simple:
- you define people and groups,
- you define how often you want to stay in touch (daily, every 2 weeks, every 3 weeks, etc.),
- the app periodically checks your call history,
- if you haven’t called someone within the defined interval, it sends a notification.

---

## How it works

- The app stores:
  - people
  - groups
  - call frequency rules
- It (will integrate) integrates with:
  - contacts
  - call history (who you called and when)
- A background process runs every 30–60 minutes and:
  - checks who should be contacted,
  - sends a notification if a call is due.

This is **not a calendar app** — it’s a lightweight (~20 MB) reminder system based on real call activity.

---

## Current state

- Core logic is fully implemented
- GUI is **not ready yet** (currently only CLI interface)
- Available only on **Linux** for now
- Configuration is stored in JSON files:
  - `people.json`
  - `groups.json`
  - `confing.json`

---

## Installation 

To build the Debian package manually, see `build/README.md`.

Alternatively, you can download the latest `.deb` package from `build/versions`
and install it using: `sudo apt install [path_to_deb_package]`.

---

## Planned features

- Proper GUI
- Android version (with contacts and call log integration)

---

## Status

This is an **early-stage project** focused on core functionality.  
Expect rough edges.