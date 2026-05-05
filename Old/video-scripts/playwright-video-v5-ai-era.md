# Playwright video — script v5 (AI era, transcript-anchored)

**Target length**: 4:30–5:00 hard cap
**Tone**: conversational host-to-camera, fast cuts to terminal/VS Code
**Audience**: SSW devs + wider dev/QA crowd, AI-curious, Playwright-curious
**Demo target site**: `ssw.com.au/rules` (Hark won't ship Rules code to LLM — agent acts as user via MCP)

> **Anchored to**: Debbie O'Brien transcripts in `Old/video-scripts/transcripts/` (8 vids). Quotes/numbers below from those.

---

## 0:00–0:20 — Intro

*Camera. Quick energy.*

> Hey, I'm Hark from SSW. Today, Playwright. But not the old Playwright. **Playwright in the AI era.**
>
> MCP servers, AI agents, a brand-new CLI. It changes how you test your UI — and how you ship UI. Let's jump in.

---

## 0:20–0:40 — What is Playwright (one breath)

*Cut to playwright.dev landing. Three boxes visible: Test, MCP, CLI.*

> Quick one. Playwright is the industry standard for browser automation and end-to-end testing. Microsoft. TypeScript, Python, .NET, Java.
>
> **Tip**: Node + TypeScript is the gold version. .NET lags on tooling and the new agents. Start there.

---

## 0:40–1:25 — Old way demo (Playwright-CTF folder)

*VS Code. `Playwright-CTF/tests/example.spec.ts` open.*

> Old way. Followed the docs. One command — `pnpm create playwright`. Got this folder. One example test, two cases. `has title`, `get started link`.
>
> Run it. `pnpm test:headed`. Watch.

*Browser pops. Test passes. Terminal green.*

> Real Chromium. Real click. Real assert. Pass.

*Edit `/Playwright/` → `/Hoaxing/`. Rerun.*

> Break it. Red. See the locator and the diff right in the terminal.

*Revert. Quick flash:*
- `pnpm test:ui` → UI mode, time-travel, picker, watch
- `pnpm test:debug` → Inspector, step-through
- VS Code Playwright extension → click element, locator generated, run from gutter

> Still works. Still useful. **But you wrote every selector.** Now watch what changes.

---

## 1:25–1:30 — The pivot

*playwright.dev — three boxes. Highlight one by one.*

> playwright.dev today. Three doors. Playwright **Test**. Playwright **MCP**. Playwright **CLI**. Plus a hidden fourth — Playwright **Test MCP**. Confusing. Sorting it now.

---

## 1:30–2:25 — Demo 1: Playwright MCP — agent as user on SSW Rules

*Terminal: `claude mcp add playwright -- npx @playwright/mcp@latest`. Show MCP loaded in Claude Code.*

> First demo. Standalone Playwright MCP. Hands the agent a real browser. Click, type, fill, scrape.
>
> Clever bit — it doesn't ship screenshots to the LLM. It ships the **accessibility tree**. Structured. Named. Deterministic. No vision model. No hallucinated coordinates.

*Prompt Claude:*

> "Open `ssw.com.au/rules`. Browse the home page like a user. Suggest one test scenario I can verify pass and fail. Don't read the source — just use the browser."

*Agent calls MCP tools. Browser opens. Agent narrates: "Found a search box. Found 'Top categories'. Suggest test — search for 'clean code' should return rule cards."*

> Now ask it to write the test.

*Prompt: "Write a Playwright test for that. Save to `tests/rules.spec.ts`."*

*Agent writes spec. `pnpm test:headed`. Browser pops. Pass.*

> Zero source code given. Agent saw the site like a user. Found locators. Wrote real Playwright TS. **That's the shift.**

---

## 2:25–3:30 — Demo 2: Playwright Test MCP + 3 agents (Planner, Generator, Healer)

*Terminal: `npx playwright init-agents --loop=claude` (or `--loop=vscode`).*

> Second demo. Different MCP. Bundled. **Playwright Test MCP** — wakes up only when these three agents run. Per-project, not global.
>
> Three agents drop into `.github/chat-modes/`:

| Agent | Job |
|---|---|
| **Planner** | Reads your live app, writes Markdown test plan |
| **Generator** | Plan → real `.spec.ts`. Verifies selectors as it writes. |
| **Healer** | Failing test → finds broken locator → patches → reruns. |

*Show the generated `seed.spec.ts` in `tests/loggedin/`.*

> One key concept — **the seed file**. Whatever you put in `seed.spec.ts` runs before every generated test. Logged-in state, fixture, navigation. The agents copy it into every spec they write. Powerful.

**Planner demo (~20s):**

*Open chat → switch to `playwright planner` agent → prompt: "Generate test plan for the rules search feature. Save as `specs/search-plan.md`."*

*Agent calls `setup_planner_page` MCP tool, browses live, explores, creates `specs/` folder, writes plan.*

> Markdown plan. Sections. Scenarios. Expected results. **Review it. Edit it. Commit it.**

**Generator demo (~20s):**

*New chat → `playwright generator` → "Generate a test file for each scenario in section 4 of the plan."*

*Agent: setup → click → `verify_element_visible` → `retrieve_test_log` → `write_test`. Per scenario.*

> Generator uses live browser to **verify locators before writing them**. No hallucinated selectors.

**Healer demo (~20s):**

*Run tests. Two fail. Switch to `healer` agent. Prompt: "Run and fix failing tests."*

*Agent: runs in debug, pauses on error, reads page snapshot, sees real DOM, patches test, re-runs to confirm.*

> Watch this. It doesn't just fix. It **re-runs to verify the fix**. And if the app is genuinely broken — it marks the test `test.fixme` with a comment. Honest.

**Why three agents not one?**
> Each has narrow tools, narrow instructions, narrow context. Smaller prompt = better output. Same reason you split prompts in Claude Projects.
>
> **Catch**: TypeScript only. Need VS Code 1.105+ and Playwright 1.56+.

---

## 3:30–4:00 — Demo 3: Playwright CLI (token-cost story)

*Terminal: `npm i -g @playwright/cli@latest` then `playwright-cli install` then `playwright-cli install --skills`.*

> Third demo. The new one. **Playwright CLI.**
>
> Problem with MCP — every page snapshot the agent looks at gets pumped into LLM context. Microsoft's own benchmark, same task:

| Tool | Tokens |
|---|---|
| Playwright MCP | **114,000** |
| Playwright CLI | **26,800** |

> Roughly **4× cheaper**. Why? CLI writes results to disk. The coding agent decides what to read into context. Headless by default.
>
> Install once globally. `install --skills` teaches Claude Code or Copilot how to use Playwright. Then your agent picks the right tool per task.

**Rule of thumb:**
- Inside a coding agent (Claude Code, Copilot)? → **CLI**
- Generic agentic loop, any MCP client, browser-as-user demos? → **MCP**

---

## 4:00–4:30 — Demo 4: Vibe-coding UI w/ Playwright feedback

*Personal site project. Claude Design mockup on left. VS Code on right.*

> Bonus. Building UIs **with** Playwright as feedback.
>
> Claude Design gave me a mockup. I want my site to match. Agent writes the component, opens the browser headless, screenshots it, compares to the design, iterates.

*Show: agent screenshots → notices header padding off → patches CSS → reruns → match.*

> This is the new Playwright dashboard — `playwright-cli show`. Scribble feedback on the screenshot. Agent receives it as PNG plus structured form. Acts on it.
>
> **Playwright before testing**, not just after. UI review loop, fully automated.

---

## 4:30–4:50 — Bonus: scraping (controversial — optional)

*Camera. Disclaimer overlay.*

> Last one. **Controversial — use ethically.** Most sites' ToS forbid scraping. Use public APIs when offered. Personal, non-commercial only.
>
> Example. I asked: do SSWers prefix their company name on LinkedIn bios — following the SSW rule? LinkedIn has no API for that.
>
> So Playwright logs in **as me** — once. Saves storage state to `auth.json`. Reuses across runs. Browses bios. Outputs JSON. Claude builds a dashboard.
>
> Big power. Use ethically. Respect ToS.

---

## 4:50–5:00 — CTA

*Camera. Three quick text overlays.*

> Decision matrix.
>
> Writing tests with AI? → **Playwright Test MCP** + 3 agents.
> Browser-as-user, scraping, UI vibe-coding? → **Playwright MCP**.
> Inside Claude Code or Copilot, care about tokens? → **Playwright CLI**.
>
> Playwright isn't a test runner anymore. It's how AI touches the web.
>
> I'm Hark from SSW. Links below. **Go ship.**

---

## Delivery notes

- "**Playwright in the AI era.**" — punch *AI*, beat
- "**That's the shift.**" — flat, not hyped
- "**It re-runs to verify the fix.**" — slow, then "And if the app is genuinely broken — it marks the test `test.fixme`." — fast
- "**114,000 tokens**" slow, "**26,800**" quick, "**4× cheaper**" punch
- "**Big power. Use ethically.**" — dead serious, no smile
- "**Go ship.**" — dry cut, no smile

## Cut list if running long (in order)

1. Drop scraping bonus (saves 20s) — most likely cut.
2. Trim Demo 4 vibe-coding to one screenshot loop (saves 15s).
3. Compress Healer demo — show only "fix → re-verify" beat (saves 15s).
4. Drop the .NET tip in Section 0:20–0:40 (saves 5s).

## On-screen / B-roll shopping list

- `Playwright-CTF/tests/example.spec.ts` open in VS Code
- `pnpm test:headed` — Chromium pop, green
- `pnpm test:headed` after `/Hoaxing/` — red diff
- `pnpm test:ui` — UI mode trace timeline
- VS Code Playwright extension — locator picker overlay
- playwright.dev homepage — three boxes side-by-side
- `claude mcp add playwright -- npx @playwright/mcp@latest`
- Claude Code chat asking agent to browse `ssw.com.au/rules`
- Generated `tests/rules.spec.ts` running green
- `npx playwright init-agents --loop=claude` output tree
- `.github/chat-modes/` showing planner.md / generator.md / healer.md
- VS Code chat picker: planner / generator / healer dropdown
- `seed.spec.ts` annotated callout
- `specs/search-plan.md` markdown rendered
- Generator writing test live, `verify_element_visible` tool call
- Healer paused on error → snapshot → patch → re-verify
- `npm i -g @playwright/cli@latest`
- Token comparison: 114k vs 26.8k bar chart (Figma)
- `playwright-cli show` dashboard with scribble feedback
- LinkedIn bio scraper — `auth.json` callout, JSON output, dashboard

## Research notes mined from transcripts

**From `Playwright Testing Agents: under the hood [HLegcP8qxVY]`:**
- VS Code 1.105+ required, Playwright 1.56+
- `--loop=` flavors: `vscode`, `claude`, `opencode`
- `seed.spec.ts` in first-project folder, copied into every generated spec
- Planner uses `setup_planner_page` MCP tool — needs the seed
- Generator uses `setup_generator_page` + `verify_element_visible` + `retrieve_test_log` + `write_test`
- Healer uses `debug_single_test` — pauses on error, reads page snapshot, can call `get_console`, `list_network_requests`
- Always commit between agent runs (Debbie does this every time) — git diff is your safety net

**From `Playwright CLI vs MCP [Be0ceKN81S8]`:**
- Exact tokens: MCP **114,000**, CLI **26,800**
- Why MCP burns tokens: full accessibility snapshots return to LLM, screenshots come back as image bytes in context
- CLI: results to disk, agent reads selectively, headless default
- CLI requires a coding agent (Claude Code / Copilot) to be efficient
- CLI is skill-based — `install --skills` adds Playwright knowledge
- MCP still wins for generic agentic loops or non-coding-agent clients

**From `[LAB] Healer Agent [PKZsdyAuuPc]`:**
- Add `--headless` to `mcp.json` args to skip browser pop during heal runs
- Healer can fix one test (drag into context) or "run and fix all failing"
- Honest mode: marks `test.fixme` when app is actually broken vs test wrong

**From `Giving UI Reviews to Coding Agents [2YWPJjOa-2w]`:**
- `playwright-cli show` opens the Playwright dashboard
- Scribble feedback on screenshot — agent receives PNG + structured form
- Dashboard shows all running background browser sessions

## Open follow-ups (not blocking script)

- Confirm exact CLI install command — `playwright-cli install` then `playwright-cli install --skills`? Verify in 5th transcript.
- Decide intro length — 0:20 or 0:15? Test on cold read.
- Test all `Playwright-CTF/package.json` scripts before recording (`pnpm test`, `:headed`, `:ui`, `:debug`, `report`, `codegen`).
