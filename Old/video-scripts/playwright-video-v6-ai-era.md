# Playwright video — script v6 (AI era, anchors new SSW rule)

**Target length**: 5:00 hard cap
**Tied to rule**: *Do you know how to use Playwright with AI Agents?* (new rule, references existing *Do you do automated UI testing?*)
**Tone**: conversational, host-to-camera, short sentences, fast cuts to terminal/VS Code
**Audience**: SSW devs + wider dev/QA crowd
**Demo target**: `ssw.com.au/rules` (public, no source given to LLM)

> **Anchored to**: Debbie O'Brien transcripts in `Old/video-scripts/transcripts/` (8 vids).
>
> **Scope changes vs v5** (per Adam + Michael Q + Dan McKay):
> - Centerpiece is now **MCP vs CLI side-by-side, same task, token cost visible**.
> - 3 agents (Planner / Generator / Healer) demoted to a brief "there's also" mention — not a full demo.
> - Scraping bonus **dropped** (Adam's call).
> - Vibe-coding section grounded in real example: my blog (Tina CMS).

---

## 0:00–0:20 — Intro

*Camera. Quick energy.*

> Hey guys, I'm Hark from SSW. Today we're learning about Playwright. But not just the old Playwright — **Playwright in the AI era**.
>
> Playwright's MCP server, AI agents, the brand-new CLI. It changes how you do UI testing. And it changes how your AI agents see the UI — not with vision and screenshots, but by reading the **accessibility tree** straight from the browser. Structured text. Roughly 200 to 400 tokens per page snapshot. No hallucinated clicks.
>
> Let's jump in.

---

## 0:20–0:45 — What is Playwright (for the new folks)

*Cut to playwright.dev. Three boxes visible: Test, MCP, CLI.*

> Quick one. Playwright is the industry standard for browser automation, end-to-end testing, and UI testing. Built by Microsoft. Available in TypeScript, Python, .NET, and Java.
>
> **Tip**. The Node SDK is the gold standard. There are other SDks, like Python, Java, .NET. I've use the .NET the core features are there, but it lags on tooling. No native test runner, weaker dev experience. If you're starting fresh, or you want to experiment, **go Node**. Trust me.

---

## 0:45–1:30 — Old way demo (`Playwright-CTF`)

*VS Code. Empty terminal split.*

> Old way. Adding Playwright is one command.

```bash
npm create playwright@latest
```

> Asks a few questions. I'll hit defaults. It scaffolds this folder — `tests/example.spec.ts`. Default test. Open browser, navigate, assert title contains "Playwright".

*Show `Playwright-CTF/tests/example.spec.ts` open.*

> I added some `package.json` scripts so I'm not typing long commands all video.

*Flash `package.json` scripts.*

```bash
pnpm test          # headless
pnpm test:headed   # browser pops
pnpm test:ui       # UI mode + trace viewer
pnpm test:debug    # inspector
```

*Run:*

```bash
pnpm test:headed
```

> Browser pops. Real Chromium. Click. Assert. Pass. Two tests, both green.

*Quick break/fix demo:*

> Break it. Change `/Playwright/` to `/Hoaxing/`.

*Run again. Red. Show error.*

> Fails as expected. Revert.

*Run `pnpm test:ui`:*

> UI mode. You get a trace viewer. Time-travel through the test. Pick locators visually.

*Close. Mention VS Code extension:*

> VS Code has a great Playwright extension. Click an element on the page, it generates the locator for you. Run tests from the gutter.

> So this is the old way. Still works. Still useful. **But you wrote every selector yourself.** Now watch what changes when AI joins.

---

## 1:30–1:35 — The pivot

*playwright.dev — three boxes visible.*

> Go to playwright.dev today. Three sections side by side. **Playwright Test**. **Playwright MCP**. **Playwright CLI**. Plus a hidden fourth — **Playwright Test MCP** — bundled with the agents.
>
> Today I want to focus on two of these. **MCP** and **CLI**. Same task. Side by side. So you can see the difference.

---

## 1:35–3:00 — Centerpiece demo: MCP vs CLI, same task, same site

*Split screen setup. Two Claude Code chats. Left = MCP. Right = CLI. Same prompt to both. Pre-installed off-camera.*

**Setup shown briefly on camera (one beat each):**

```bash
# Left chat — install Playwright MCP
claude mcp add playwright -s user -- npx -y @playwright/mcp@latest
```

```bash
# Right chat — install Playwright CLI
npm install -g @playwright/cli
playwright-cli install --skills   # optional — teaches Claude Code the commands
```

> Two installs. MCP — adds an MCP server. The agent gets a set of structured tools. CLI — installs a binary, plus optionally a Skills pack. The agent runs shell commands and reads results from disk.
>
> Note: CLI needs Node 18 plus, and a coding agent that can read your file system — Claude Code, GitHub Copilot, Cursor with file access. Skills are optional. Without them, the agent just discovers commands via `playwright-cli --help`.

**The prompt (paste both sides):**

> "Open `https://www.ssw.com.au/rules/`. Browse like a user. Suggest one test scenario I can verify pass and fail. Then write the Playwright test for it. Save it to `tests/rules.spec.ts`. Don't read source — only use the browser."

**Run both. Watch them work.**

*Left (MCP):* Agent calls `browser_navigate`, `browser_snapshot`, `browser_click`. Each snapshot streams back into the chat context.

*Right (CLI):* Agent runs `playwright-cli` commands. Output goes to disk. Agent reads only what it needs.

> Both finish. Both write a working test. Run both:

```bash
pnpm test:headed tests/rules.spec.ts
```

> Both pass. Same result.

**Now the cost.**

*Open Claude Code's context indicator (Ctrl+R or `/context` — show the live token usage).*

> Same task. MCP burned **roughly 114,000 tokens**. CLI — **27,000**. About **four times cheaper**.

*Token comparison overlay (Figma graphic).*

| Tool | Tokens for the same task |
|---|---|
| Playwright MCP | **~114,000** |
| Playwright CLI | **~27,000** |

> Microsoft's own numbers, by the way. From the Playwright team's CLI announcement video.

**Why the gap?**

> Where the browser state lives. With **MCP**, state lives in the LLM's context window — every snapshot, every tool schema, every result streams back into the model. With **CLI**, state lives on disk — the coding agent reads only what it needs into context.
>
> Note — Playwright MCP is already efficient compared to most browser MCPs. It sends the accessibility tree as structured text, not screenshots. About **200 to 400 tokens per snapshot**. But on a long task with many tool calls and tool schemas, it still adds up.

**So why does MCP exist if CLI is cheaper?**

> Per Microsoft's own docs — **CLI is best for coding agents working with large codebases**. Token-efficient, skill-based.
>
> **MCP is best for specialized agentic loops that benefit from persistent state and iterative reasoning over page structure** — exploratory automation, long-running autonomous workflows, anywhere the LLM needs the page snapshot directly in context to reason about what to do next.
>
> Practical translation:
> - **MCP works with any MCP client** — VS Code, Cursor, Windsurf, Claude Desktop, Claude Code, your own agentic loop. CLI needs a coding agent with file-system access. So if your agent isn't a coding agent, MCP is your only option.
> - **MCP keeps session state alive in the chat**. The agent reasons over the live page step by step. CLI is one-shot per command — fine for batch automation, less natural for "browse and decide".

**The rule.**

> Inside Claude Code, GitHub Copilot, Cursor — building tests, generating code, running long agent jobs? → **CLI**. Token-cheap, headless by default.
> Exploratory, interactive browse-as-user, or any non-coding-agent MCP client? → **MCP**.
>
> Both can coexist. Pick the right one per task.

---

## 3:00–3:30 — There's also: Playwright Test MCP + 3 agents

*Cut. Camera. Quick beat.*

> Quick mention before I move on. There's a **fourth Playwright tool** that's specifically for testing — **Playwright Test MCP**. Not the same as the standalone MCP. This one ships three AI agents:

| Agent | Job |
|---|---|
| **Planner** | Reads your live app, writes a Markdown test plan |
| **Generator** | Plan → real Playwright TypeScript. Verifies selectors live. |
| **Healer** | Failing test → finds the broken locator → patches → re-runs. |

> One command sets it all up:

```bash
npx playwright init-agents --loop=claude
```

> Or `--loop=vscode`, or `--loop=opencode`. It generates agent definitions in `.github/chat-modes/`, plus a seed test that runs before every generated spec.
>
> **Catch** — TypeScript only, for now. Need Playwright 1.56 plus, VS Code 1.105 plus.
>
> I'm not running the full demo here — Debbie O'Brien from the Playwright team has a great deep-dive on her channel. Linked below. The point is: when you're ready to scale, these agents can write **75 percent of your tests for you**. Real proof — she put 107 tests on her movies app, 75 percent agent-written.

---

## 3:30–4:20 — Beyond testing: Playwright as a feedback loop for AI

*Cut to my blog repo. Tina CMS visible. Claude Design mockup on screen.*

> One more thing — and this is the part that surprised me most. **Playwright isn't just for testing anymore.**
>
> I'm building my personal blog with Tina CMS. I'm not a frontend designer. I want a great-looking site. So I asked Claude Design to give me a mockup. Now I want my live site to actually match that design.
>
> Here's the new bit. I tell Claude — "Use Playwright to **see** what you're building. Don't guess. Open the browser. Read the accessibility tree. Compare against the design. Iterate."

*Screen: Claude Code session. Agent writes a component → calls Playwright (MCP or CLI) → opens browser headless → reads accessibility tree → screenshots → compares → patches CSS → reruns.*

> Watch what's happening. The agent isn't using vision. It's not just looking at a screenshot. It's reading the **accessibility tree** — every heading, every landmark, every element with a role and name. Same way a screen reader sees the page. Structured. Deterministic.
>
> If a button is missing alt text, it knows. If the responsive layout is broken, it knows. If a network call fails — it pulls the network log. If there's a console error — it reads the console. **Playwright gives the agent eyes, ears, and dev tools, all in one.**
>
> And it's not just for designing UIs. Same loop works for:
> - **Filling forms** — agent navigates, fills, submits
> - **Login flows** — Playwright saves storage state to a JSON, reuses across runs
> - **Browser automation** — anything you'd do manually in a browser
> - **Debugging APIs** — network log access, console access
>
> So Playwright in the AI era isn't just "test runner that AI uses". It's how AI agents **touch the web**.

---

## 4:20–4:50 — Decision matrix + new SSW rule

*Camera. Three text overlays one at a time.*

> So which Playwright tool do you reach for?
>
> Writing tests with AI inside Claude Code, Copilot, Cursor? → **Playwright Test + CLI**, or the three agents if you want full automation.
> Driving a browser from any MCP client, or building UIs with AI as your senior dev? → **Playwright MCP**.
> Care about tokens and you're inside a coding agent? → **CLI** every time.

*New SSW rule overlay: "Do you know how to use Playwright with AI Agents?"*

> SSW just added a new rule for this — **"Do you know how to use Playwright with AI Agents?"** It pairs with our existing rule, "Do you do automated UI testing?". Link in the description. Both rules now reference each other.
>
> Playwright isn't a test runner anymore. It's how AI agents touch the web.
>
> I'm Hark from SSW. **Go ship.**

---

## Delivery notes

- "**Playwright in the AI era**" — punch *AI*, beat after.
- "**Trust me.**" after the Node SDK line — half-smile, dead beat.
- "**No hallucinated clicks**" — flat, certain.
- "**Roughly 114,000 tokens**" slow, "**27,000**" quick — contrast.
- "**About four times cheaper**" — punch *cheaper*.
- "**Microsoft's own numbers**" — flat, factual.
- "**Playwright gives the agent eyes, ears, and dev tools, all in one.**" — slow, separate beats per phrase.
- "**Go ship.**" — dry cut, no smile.

## Cut list if running long (in order)

1. Trim the "old way" break/fix sequence — show pass once, mention you can edit and rerun, skip the red shot. Saves ~20s.
2. Drop the "filling forms / login / debugging APIs" bullet list at end of vibe-coding section. Saves ~10s.
3. Trim the SDK tip in 0:20–0:45 to one sentence: "Use the Node TypeScript SDK. It's the gold standard." Saves ~5s.
4. Drop the proof point — "75 percent of 107 tests" — keeps the structure, loses the credibility hit. Last resort.

## On-screen / B-roll shopping list

- `npm create playwright@latest` running fresh, accepting defaults
- `Playwright-CTF/tests/example.spec.ts` open in VS Code
- `package.json` scripts highlighted
- `pnpm test:headed` — Chromium pop, green
- `pnpm test:headed` after `/Hoaxing/` — red, terminal diff
- `pnpm test:ui` — UI mode trace viewer with hover
- VS Code Playwright extension — locator picker overlay
- playwright.dev homepage — three boxes side by side
- Two Claude Code chats split-screen, MCP install left + CLI install right
- `claude mcp add playwright ...` and `claude mcp list` showing connected
- `npm i -g @playwright/cli@latest` and `playwright-cli install --skills`
- Both chats running same prompt — MCP tool calls vs CLI shell calls
- `pnpm test:headed tests/rules.spec.ts` — both passing
- Token comparison graphic — bar chart, MCP 114k vs CLI 27k (Figma)
- `npx playwright init-agents --loop=claude` — output tree
- `.github/chat-modes/` showing planner.md / generator.md / healer.md
- Blog repo with Tina CMS — Claude Design mockup side by side with live site
- Claude Code agent reading accessibility tree, patching CSS, rerunning
- New SSW rule overlay: "Do you know how to use Playwright with AI Agents?"
- Cross-link overlay: existing "Do you do automated UI testing?" rule

## How to show token cost on camera (Claude Code)

In Claude Code, the live context usage shows in the bottom status bar of the chat. To make it visible during the demo:
- Use `/context` to print the breakdown
- Or look at the percentage indicator in the chat header
- Pre-test which one renders best on camera

Backup: pre-bake the Figma graphic. If live numbers are noisy, cut to the graphic.

## Open follow-ups before recording

- [ ] Verify exact `playwright-cli` command names by running `playwright-cli --help` once before camera (Debbie's transcript may be ahead of the released version)
- [ ] Confirm new SSW rule URL exists by record day — overlay needs a real link
- [ ] Confirm cross-link is live on existing "Do you do automated UI testing?" rule
- [ ] Pre-test `ssw.com.au/rules` for cookie banner / Cloudflare challenge in fresh Chrome profile — agent needs clean access
- [ ] Pre-bake Figma token comparison graphic
- [ ] Pre-bake decision matrix overlay
- [ ] Sanity check: chromium binary installed (`pnpm exec playwright install chromium`)

## Sources / fact-check (for claims made on camera)

Every technical claim in this script is grounded here. Update if Microsoft changes wording.

| Claim in script | Source | Direct quote / number |
|---|---|---|
| MCP reads accessibility tree, not pixels | [playwright.dev/mcp/introduction](https://playwright.dev/mcp/introduction) | "operates on the accessibility tree, not pixels" |
| ~200–400 tokens per snapshot | [playwright.dev/mcp/introduction](https://playwright.dev/mcp/introduction) | "~200-400 tokens per snapshot vs thousands for DOM/screenshots" |
| MCP works with VS Code, Cursor, Windsurf, Claude Code, Claude Desktop, any MCP client | [playwright.dev/mcp/introduction](https://playwright.dev/mcp/introduction) | "Works everywhere — VS Code, Cursor, Windsurf, Claude Code, Claude Desktop, and any MCP client" |
| MCP has 40+ tools | [playwright.dev/mcp/introduction](https://playwright.dev/mcp/introduction) | "40+ tools spanning navigation, form filling, network mocking…" |
| 114k vs 27k tokens, ~4× | [Playwright CLI vs MCP video](https://www.youtube.com/watch?v=Be0ceKN81S8) (Microsoft / Debbie O'Brien) | "MCP… 114,000 tokens… CLI has taken only 26.8 thousand tokens" |
| CLI best for coding agents w/ large codebases | [playwright.dev/agent-cli/introduction](https://playwright.dev/agent-cli/introduction) | "best for coding agents (Claude Code, Copilot) working with large codebases" |
| MCP best for specialized agentic loops, persistent state, iterative reasoning | [playwright.dev/agent-cli/introduction](https://playwright.dev/agent-cli/introduction) | "specialized agentic loops that benefit from persistent state and iterative reasoning over page structure, such as exploratory automation or long-running autonomous workflows" |
| CLI requires Node 18+ and a coding agent w/ FS access | [playwright.dev/docs/getting-started-cli](https://playwright.dev/docs/getting-started-cli) | "Claude Code, GitHub Copilot… Node.js 18+" |
| CLI install: `npm install -g @playwright/cli` | [playwright.dev/agent-cli/introduction](https://playwright.dev/agent-cli/introduction) | exact command from official docs |
| Skills are optional — agents can discover via `--help` | [playwright.dev/docs/getting-started-cli](https://playwright.dev/docs/getting-started-cli) | "discover commands on its own" without skills |
| Test agents (Planner/Generator/Healer) — TS only, Playwright 1.56+, VS Code 1.105+ | [Playwright Testing Agents under the hood video](https://www.youtube.com/watch?v=HLegcP8qxVY) (Microsoft / Debbie O'Brien) | direct from transcript |
| `init-agents --loop=` flavors: vscode, claude, opencode | same video | direct from transcript |
| Healer marks `test.fixme` when app actually broken | same video | direct from transcript |
| 75% of 107 tests agent-written on Debbie's movies app | same video | direct from transcript |

**Things I am NOT claiming on camera (avoid stupid mistakes):**

- Don't claim CLI is "always" cheaper — only on long sessions with many tool calls. Single-shot tasks can be similar.
- Don't claim MCP is "obsolete" — Microsoft explicitly positions both as complementary.
- Don't claim CLI works without a coding agent. It needs file-system access. Generic LLM clients (Claude Desktop alone) can't use CLI effectively.
- Don't claim accessibility-tree-only — MCP can also take screenshots when explicitly asked, but they go to disk by default in newer versions. Quoting "no vision needed" only as the *default* MCP path.
- Don't claim `init-agents` works for Python/Java/.NET. TypeScript only as of Playwright 1.56.
