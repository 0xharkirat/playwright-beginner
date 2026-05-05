# Playwright video — script v7 (AI era, single-demo flow)

**Target length**: ~5 min hard cap (delivered fast)
**Tied to rule**: *Do you know how to use Playwright with AI Agents?* (new SSW rule, references existing *Do you do automated UI testing?*)
**Tone**: conversational, host-to-camera, short sentences, fast cuts to terminal/VS Code
**Audience**: SSW devs + wider dev/QA crowd
**Demo target**: `tina.io` (homepage, hamburger menu, nav)

> **Scope changes vs v6**:
> - Dropped side-by-side MCP vs CLI demo — was too academic. Replaced with a **single coherent MCP demo** that mirrors Debbie's flow.
> - Demo target moved from `ssw.com.au/rules` (was broken at record time) to `tina.io`.
> - New beat: **manual break → ask AI to fix** — teaches healer-style workflow without spinning up the Healer agent.
> - CLI shown briefly *after* MCP, with token-efficiency screenshot from Microsoft's official talk.
> - 3 agents kept as a brief "and there's also this" mention.
> - Vibe-coding callback specifically tied to my Tina CMS blog.

---

## 0:00–0:20 — Intro

*Camera. Quick energy.*

> Hey guys, I'm Hark from SSW. Today we're learning about Playwright. But not just the old Playwright — **Playwright in the AI era**.
>
> Playwright's MCP server, AI agents, the brand-new CLI. It changes how you do UI testing. And it changes how your AI agents see the UI — not with vision and screenshots, but by reading the **accessibility tree** straight from the browser. Structured text. Around 200 to 400 tokens per page snapshot. No hallucinated clicks.
>
> Let's jump in.

---

## 0:20–0:45 — What is Playwright (for the new folks)

*Cut to playwright.dev. Three boxes visible: Test, MCP, CLI.*

> Quick one. Playwright is the industry standard for browser automation, end-to-end testing, and UI testing. Built by Microsoft. Available in TypeScript, Python, .NET, and Java.
>
> **Tip**. The Node SDK is the gold standard. There are other SDKs — Python, Java, .NET. I've used the .NET one — the core features are there, but it lags on tooling. No native test runner, weaker dev experience. If you're starting fresh or want to experiment, **go Node**. Trust me.

---

## 0:45–1:15 — Old way demo (`Playwright-CTF`)

*VS Code. Empty terminal split.*

> Old way. Adding Playwright is one command.

```bash
npm create playwright@latest
```

> Asks a few questions. I hit defaults. It scaffolds `tests/example.spec.ts`. Default test — open browser, navigate, assert title contains "Playwright".

*Show `Playwright-CTF/tests/example.spec.ts`. Run:*

```bash
pnpm test:headed
```

> Browser pops. Real Chromium. Real click. Real assert. Pass.

*Quick mention — don't run all of these in full:*

> You also get **UI mode** with a trace viewer. **Debug mode** with the Playwright Inspector. And the VS Code extension lets you click an element on the page to generate the locator.
>
> So this is the old way. Still works. Still useful. **But you wrote every selector yourself.** Now watch what changes when AI joins.

---

## 1:15–1:25 — The pivot

*playwright.dev — three boxes visible.*

> Go to playwright.dev today. Three sections side by side. **Playwright Test**. **Playwright MCP**. **Playwright CLI**. Plus a hidden fourth — **Playwright Test MCP** — bundled with the test agents.
>
> I'll start with the most powerful one for exploring — **Playwright MCP**.

---

## 1:25–3:15 — Centerpiece demo: AI explores, suggests, writes — you run

*Claude Code. Empty chat. Terminal visible.*

**The prompt** *(paste exactly — show on screen briefly so viewers can copy):*

> ```
> I want to write a Playwright test for tina.io. Do these steps in order:
>
> 1. Check if Playwright MCP server is installed for this Claude Code instance.
>    If not, install it for me at user scope.
>
> 2. Once MCP is ready, open tina.io in a browser. Explore the home page like
>    a real user — header, all menu items, the hamburger menu, footer links.
>    Don't read source code. Only use the browser.
>
> 3. Suggest exactly ONE test scenario I can verify. Something simple but
>    real — a navigation flow, a visible heading, a working link.
>
> 4. Close the browser when you're done exploring.
>
> 5. Update package.json with simple scripts:
>    - "test"        → headless chromium
>    - "test:headed" → headed chromium so I can watch
>    - "test:ui"     → UI mode
>
> 6. Write the test file. Save it to tests/tina.spec.ts. Don't run it yet.
> ```

**On camera:**

1. Paste prompt. Hit send.
2. Agent does step 1: runs `claude mcp list`, sees nothing, runs `claude mcp add playwright -s user -- npx -y @playwright/mcp@latest`, allow.
3. Agent does step 2: calls `browser_navigate("https://tina.io")`, then `browser_snapshot`, then `browser_click` on hamburger, more snapshots, exploring.
4. Agent does step 3: writes a one-paragraph scenario suggestion in the chat — e.g., *"Click the hamburger, click 'Docs', verify URL changes and a 'Getting Started' heading appears."*
5. Agent does step 4: closes the browser.
6. Agent does step 5: updates `package.json` scripts.
7. Agent does step 6: writes `tests/tina.spec.ts`.

> See what just happened? **The agent acted as a user.** No source code. No selector hand-picking. It found the page, the hamburger, the docs link — and chose a real test.

**Now run it yourself:**

*Open `tests/tina.spec.ts` — show it briefly.*

*In terminal:*

```bash
pnpm test:headed
```

*Browser pops. Test runs.*

> Two things can happen now. It passes. Or it fails. AI is non-deterministic — I don't know which we'll see live.

**Branch A — it passes (likely):**

> Pass. Cool. Now let me **break it on purpose** to show you the next bit.

*Edit `tests/tina.spec.ts` — change a locator name or expected text to something wrong. Save.*

```bash
pnpm test:headed
```

*Red. Test fails.*

> Now the prompt that matters. Don't fix it yourself. **Ask Claude.**

*New chat or same chat:*

> ```
> The test in tests/tina.spec.ts is failing. Use Playwright MCP to figure out
> why. Look at the actual page. Then fix the test. Don't change the assertion
> intent — keep what we're checking, just fix the locator.
> ```

> Claude opens the browser, reads the live page, sees the locator changed, patches the test, re-runs it.

**Branch B — it fails on first run:**

> Fails. Same thing — let Claude fix it. **But notice what we did NOT do**. We did not let Claude auto-fix while writing. We let it write, we ran it ourselves, we judged the result. **Human in the loop.**

> This is the new workflow. AI writes the boring scaffolding. **You stay in control.** Better than writing every selector by hand. Way better than auto-pilot.

---

## 3:15–3:45 — There's also: Playwright CLI (token cost)

*Cut to terminal.*

> Same workflow — but inside Claude Code with file system access — there's a cheaper way. **Playwright CLI.**

*Brief install demo:*

```bash
npm install -g @playwright/cli
playwright-cli install --skills   # optional — teaches Claude the commands
```

> CLI runs the same browser primitives — navigate, click, snapshot. **The difference is where the data lives.**
>
> MCP streams every snapshot back into your chat context. CLI writes results to disk. Your coding agent reads only what it needs.

*Cut to Microsoft's screenshot — token comparison from the official Playwright CLI vs MCP video.*

| Tool | Tokens for the same task |
|---|---|
| Playwright MCP | **~114,000** |
| Playwright CLI | **~27,000** |

> Microsoft's own benchmark. Same task. **About four times cheaper.**

**So if CLI is cheaper, why does MCP exist?**

> Per Microsoft's own docs:
> - **CLI** is best for *coding agents working with large codebases* — Claude Code, Copilot. Token-efficient.
> - **MCP** is best for *specialized agentic loops with persistent state and iterative reasoning over page structure* — exploring, browsing as a user, anywhere the LLM benefits from having the live page in context.
>
> Practical translation:
> - **Exploring like a user, suggesting tests, debugging interactively?** → MCP. The agent reasons over the live page step by step. That's what we just did.
> - **Generating, running, iterating tests inside a coding agent?** → CLI. Cheaper, headless by default.
> - **Not using a coding agent at all** (Claude Desktop, generic MCP client)? → MCP. CLI needs file-system access.
>
> My take — **start with CLI for cost, reach for MCP when you need to explore.** Both can coexist.

---

## 3:45–4:15 — There's also: Playwright Test MCP + 3 agents

*Cut. Camera. Quick beat.*

> One more piece. Playwright also ships **three test-specific AI agents** — wrapped in their own bundled MCP server called Playwright Test MCP.

| Agent | Job |
|---|---|
| **Planner** | Reads your live app, writes a Markdown test plan |
| **Generator** | Plan → real Playwright TypeScript. Verifies selectors live. |
| **Healer** | Failing test → finds the broken locator → patches → re-runs. |

> One command sets it all up:

```bash
npx playwright init-agents --loop=claude
```

> Or `--loop=vscode`, or `--loop=opencode`. **TypeScript only**, Playwright 1.56 plus.
>
> I'm not running these full demos — Debbie O'Brien from the Playwright team has a great deep-dive on her channel. Linked below. Net result — she put 107 tests on her movies app, **75 percent agent-written**.

---

## 4:15–4:45 — Beyond testing: Playwright as a feedback loop for AI

*Cut to my blog repo. Tina CMS visible. Claude Design mockup on screen.*

> One last thing — and this is what surprised me most. **Playwright isn't just for testing anymore.**
>
> Callback — I'm building my personal blog with **Tina CMS**. Markdown content, Next.js, file-based. I'm not a frontend designer. I want a great-looking site. So I gave Claude a Tina-themed design and said: **build it, then use Playwright to check your own work.**

*Screen: Claude Code session. Agent writes a component → calls Playwright (CLI by default for cost, MCP when it needs to explore) → opens browser headless → reads accessibility tree → screenshots → compares → patches CSS → reruns.*

> Watch the loop. The agent isn't using vision. It's reading the **accessibility tree** — every heading, every landmark, every element with a role and a name. Same way a screen reader sees the page. Structured. Deterministic.
>
> If a button is missing alt text — it knows. If the responsive layout breaks — it knows. If the API fails — it pulls the network log. If there's a console error — it reads the console.
>
> **Playwright gives the agent eyes, ears, and dev tools, all in one.**
>
> And it's not only for designing UIs. Same loop works for filling forms, login flows, browser automation, API debugging — anywhere you'd touch a browser by hand.

---

## 4:45–5:00 — Decision matrix + new SSW rule

*Camera. Three text overlays.*

> So which Playwright tool do you reach for?
>
> Inside Claude Code, Copilot, Cursor — generating, running, iterating tests? → **CLI** first, for cost.
> Exploring a site, suggesting tests, browsing as a user, debugging interactively? → **MCP**.
> Want full automation — plan, generate, heal? → **The three agents**.

*New SSW rule overlay: "Do you know how to use Playwright with AI Agents?"*

> SSW just added a new rule for this — **"Do you know how to use Playwright with AI Agents?"** It pairs with our existing rule, "Do you do automated UI testing?". Both rules cross-reference. Link in the description.
>
> Playwright isn't a test runner anymore. **It's how AI agents touch the web.**
>
> I'm Hark from SSW. **Go ship.**

---

## Delivery notes

- "**Playwright in the AI era**" — punch *AI*, beat after.
- "**Trust me.**" after the Node SDK line — half-smile, dead beat.
- "**No hallucinated clicks**" — flat, certain.
- "**The agent acted as a user.**" — slow, separate beats.
- "**Human in the loop.**" — punch each word.
- "**About four times cheaper**" — punch *cheaper*.
- "**Microsoft's own benchmark**" — flat, factual.
- "**Playwright gives the agent eyes, ears, and dev tools, all in one.**" — slow, beats per phrase.
- "**It's how AI agents touch the web.**" — slow, then "**Go ship.**" — dry cut.

## Cut list if running long (in order)

1. Drop the manual-break-then-fix branch (3:00–3:15) — show only the happy-path test pass. Saves ~30s.
2. Trim CLI section to one beat: "There's also CLI, ~4× cheaper, here's when". Saves ~15s.
3. Drop Branch B (test fails on first run) — only show Branch A. Saves ~10s.
4. Drop the SDK tip in 0:20–0:45 to one sentence. Saves ~5s.

## On-screen / B-roll shopping list

- `npm create playwright@latest` running fresh, accepting defaults
- `Playwright-CTF/tests/example.spec.ts` open in VS Code
- `pnpm test:headed` — Chromium pop, green
- `pnpm test:ui` flash — UI mode trace viewer
- VS Code Playwright extension — locator picker overlay (flash)
- playwright.dev homepage — three boxes side by side
- The detailed prompt — paste it on screen so viewers can copy
- Claude Code agent running `claude mcp add playwright …` live
- MCP browser tool calls — `browser_navigate`, `browser_snapshot`, `browser_click`
- Updated `package.json` with the three new scripts
- Generated `tests/tina.spec.ts` open in VS Code
- `pnpm test:headed` — Chromium runs against tina.io, green
- Manual break — edit a locator, save, rerun, red
- "Fix this test" prompt to Claude — agent uses MCP to inspect, fix, rerun, green
- `npm install -g @playwright/cli` and `playwright-cli install --skills`
- Microsoft's screenshot from the CLI vs MCP video — token comparison (114k vs 27k)
- `npx playwright init-agents --loop=claude` — output tree
- `.github/chat-modes/` showing planner.md / generator.md / healer.md
- Blog repo with Tina CMS — Claude Design mockup side by side with live site
- Claude Code agent reading accessibility tree, patching CSS, rerunning
- New SSW rule overlay: "Do you know how to use Playwright with AI Agents?"
- Cross-link overlay: existing "Do you do automated UI testing?" rule

## How to show token cost on camera (Claude Code)

Live token usage shows in chat header / bottom status bar. To make it visible:
- Use `/context` to print the breakdown
- Or look at the percentage indicator in the chat header
- Pre-test which renders best on camera

**Backup**: pre-bake a still from Microsoft's own video frame (Debbie's CLI vs MCP video timestamp ~0:55 has the side-by-side token panel). Cut to that frame. Cleaner than live.

## Open follow-ups before recording

- [ ] Verify exact `playwright-cli` command names by running `playwright-cli --help` once before camera
- [ ] Confirm new SSW rule URL exists by record day — overlay needs a real link
- [ ] Confirm cross-link is live on existing "Do you do automated UI testing?" rule
- [ ] Pre-test `tina.io` for cookie banner / chat-bot popups — agent needs clean access to hamburger and nav
- [ ] Decide test scenario you *want* the agent to land on, in case you need to nudge it (e.g., "verify Docs link in nav opens /docs page with 'Getting Started' visible")
- [ ] Pre-bake decision matrix overlay
- [ ] Capture Microsoft's CLI vs MCP screenshot for the token-comparison cut
- [ ] Sanity check: chromium binary installed (`pnpm exec playwright install chromium`)
- [ ] Demo machine: clean MCP state (`claude mcp list` → nothing playwright-related) so install demo is authentic
- [ ] Demo machine: `playwright-cli` not on PATH, so the CLI install demo is authentic

## Sources / fact-check (for claims made on camera)

| Claim in script | Source | Direct quote / number |
|---|---|---|
| MCP reads accessibility tree, not pixels | [playwright.dev/mcp/introduction](https://playwright.dev/mcp/introduction) | "operates on the accessibility tree, not pixels" |
| ~200–400 tokens per snapshot | [playwright.dev/mcp/introduction](https://playwright.dev/mcp/introduction) | "~200-400 tokens per snapshot vs thousands for DOM/screenshots" |
| MCP works with VS Code, Cursor, Windsurf, Claude Code, Claude Desktop, any MCP client | [playwright.dev/mcp/introduction](https://playwright.dev/mcp/introduction) | "Works everywhere — VS Code, Cursor, Windsurf, Claude Code, Claude Desktop, and any MCP client" |
| 114k vs 27k tokens, ~4× | [Playwright CLI vs MCP video](https://www.youtube.com/watch?v=Be0ceKN81S8) (Microsoft / Debbie O'Brien) | "MCP… 114,000 tokens… CLI has taken only 26.8 thousand tokens" |
| CLI best for coding agents w/ large codebases | [playwright.dev/agent-cli/introduction](https://playwright.dev/agent-cli/introduction) | "best for coding agents (Claude Code, Copilot) working with large codebases" |
| MCP best for specialized agentic loops, persistent state, iterative reasoning | [playwright.dev/agent-cli/introduction](https://playwright.dev/agent-cli/introduction) | "specialized agentic loops that benefit from persistent state and iterative reasoning over page structure, such as exploratory automation or long-running autonomous workflows" |
| CLI requires Node 18+ and a coding agent w/ FS access | [playwright.dev/docs/getting-started-cli](https://playwright.dev/docs/getting-started-cli) | "Claude Code, GitHub Copilot… Node.js 18+" |
| CLI install: `npm install -g @playwright/cli` | [playwright.dev/agent-cli/introduction](https://playwright.dev/agent-cli/introduction) | exact command from official docs |
| Skills are optional — agents can discover via `--help` | [playwright.dev/docs/getting-started-cli](https://playwright.dev/docs/getting-started-cli) | "discover commands on its own" without skills |
| Test agents (Planner/Generator/Healer) — TS only, Playwright 1.56+ | [Playwright Testing Agents under the hood](https://www.youtube.com/watch?v=HLegcP8qxVY) | direct from transcript |
| `init-agents --loop=` flavors: vscode, claude, opencode | same video | direct from transcript |
| 75% of 107 tests agent-written on Debbie's movies app | same video | direct from transcript |

**Things I am NOT claiming on camera:**

- Don't claim CLI is "always" cheaper — only on long sessions with many tool calls.
- Don't claim MCP is "obsolete" — Microsoft positions both as complementary.
- Don't claim CLI works without a coding agent.
- Don't claim "no vision needed ever" — MCP can take screenshots when explicitly asked. The default path is accessibility tree only.
- Don't claim `init-agents` works for Python/Java/.NET. TypeScript only.
