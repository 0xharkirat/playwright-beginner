# Playwright video — script v9 (final structure)

**Target length**: 5–7 min (no hard cap — pace driven by content, not clock)
**Tied to rule**: *Do you know how to use Playwright with AI Agents?* (new SSW rule, references existing *Do you do automated UI testing?*)
**Tone**: conversational, host-to-camera, short sentences, fast cuts to terminal/VS Code
**Audience**: SSW devs + wider dev/QA crowd

> **Structure (final)**:
> 1. Intro
> 2. What is Playwright
> 3. **Old way demo** — write a test by hand, run it
> 4. **Demo 1 — AI writes the test (MCP)** — tina.io, explore as user, suggest, write, run
> 5. **Demo 2 — MCP vs CLI** — same idea, two sub-demos showing where each wins
>    - 2a (MCP wins): automationexercise.com — explorative shopping
>    - 2b (CLI wins): batch screenshots of 5 CMS homepages
> 6. **Three test agents** — Planner / Generator / Healer — brief mention, point at Debbie's video
> 7. **Vibe-coding callback** — Tina blog feedback loop with Playwright as eyes/ears/dev tools
> 8. CTA + new SSW rule
>
> Scope changes vs v8:
> - Demo 1 now MCP-only on tina.io (the "AI writes a test for me" beat)
> - MCP vs CLI moved to Demo 2 with two clear sub-demos
> - **Scraping dropped entirely** (Hark called — controversial, out)
> - Added callback to Hark's prior 20-min Tina blog setup video at end
> - Added SSW dojo + Debbie's channel as resources

---

## 0:00–0:20 — Intro

*Camera. Quick energy.*

> Hey guys, I'm Hark from SSW. Today we're learning about Playwright. But not just the old Playwright — **Playwright in the AI era**.
>
> Playwright's MCP server, AI agents, the brand-new CLI. It changes how you do UI testing. And it changes how your AI agents see the UI — not with vision and screenshots, but by reading the **accessibility tree** straight from the browser. Structured text. Around 200 to 400 tokens per page snapshot. No hallucinated clicks.
>
> Let's jump in.

---

## 0:20–0:40 — What is Playwright

*Cut to playwright.dev. Three boxes visible: Test, MCP, CLI.*

> Quick one. Playwright is the industry standard for browser automation, end-to-end testing, and UI testing. Built by Microsoft. TypeScript, Python, .NET, Java.
>
> **Tip**. Node SDK is the gold standard. Other SDKs lag on tooling — no native test runner, weaker dev experience. If you're starting fresh, **go Node**. Trust me.

---

## 0:40–1:20 — Old way demo

*VS Code. `Playwright-CTF/tests/example.spec.ts` open.*

> Old way. One command — `npm create playwright`. Hit defaults. It scaffolds a default test in `tests/example.spec.ts`. Open the browser, navigate to playwright.dev, assert the title contains "Playwright".
>
> Run it.

```bash
pnpm test:headed
```

> Real Chromium pops. Click. Assert. Pass.
>
> Quick tour of what else you get:
> - **UI mode** — a trace viewer with time-travel
> - **Debug mode** — the Playwright Inspector, step through
> - **VS Code extension** — click an element on the page, locator generated, run from the gutter
>
> So you can write tests by hand. You can use the locator picker. **All useful — but you're still writing every selector yourself.** Now watch what changes when AI joins.

---

## 1:20–2:30 — Demo 1: AI writes the test (Playwright MCP on tina.io)

*Claude Code. Empty chat. Terminal visible.*

**Setup beat:**

```bash
claude mcp add playwright -s user -- npx -y @playwright/mcp@latest
```

> MCP installed. Claude Code now has browser tools.

**The prompt** *(paste on screen):*

> ```
> I want to explore the home page of https://tina.io as a real user.
> Use Playwright MCP. Open the page, dismiss any cookie banner if it
> shows up, walk through the header, hamburger menu, and main CTAs.
> Then suggest exactly ONE simple test scenario I can verify — something
> like a navigation flow, a visible heading, or a working CTA.
>
> When you're done, write the Playwright test for it. Save it to
> tests/tina.spec.ts. Don't run it — I'll run it myself.
> ```

**On camera:**

1. Paste prompt. Send.
2. Agent calls `browser_navigate("https://tina.io")`.
3. Snapshot — agent sees cookie banner, dismisses it.
4. Snapshot — agent walks the header, opens hamburger, reads menu items.
5. Agent reports: *"Suggested scenario — clicking 'Docs' in the nav should land on /docs and show a heading containing 'Documentation'."*
6. Agent writes `tests/tina.spec.ts`.

> See what happened. **The agent acted as a user.** No source code. No selector hand-picking. It explored the live page, picked a real flow, wrote the test.
>
> Now I run it myself.

```bash
pnpm test:headed tests/tina.spec.ts
```

*Browser pops. Test runs.*

> Pass. Test written by AI. Verified by me. **Human in the loop.**
>
> This is the new workflow. AI writes the boring scaffolding. You judge the result.

---

## 2:30–4:30 — Demo 2: MCP vs CLI — same idea, two scenarios

*Camera. Brain vs Hard Drive overlay.*

> Quick segue. With the latest Playwright release, Microsoft also shipped a brand-new tool — **Playwright CLI**. So now you have a choice. **MCP or CLI.**
>
> The difference in one sentence — **MCP is stateful but token-heavy. CLI is stateless but token-cheap.**

| | MCP — "Brain" | CLI — "Hard Drive" |
|---|---|---|
| State | Lives in LLM context window | Lives on local filesystem |
| Each step | Snapshot streamed into context | Result saved to a file |
| Best for | **Exploration**, dynamic flows, reasoning step-by-step | **Batch jobs**, repetitive, code generation |

> Same Chromium underneath. Same primitives. The difference is **where the data goes**.
>
> Two scenarios — one where MCP wins, one where CLI wins.

---

### 2:45–3:30 — 2a: Where MCP wins (explorative shopping)

*Cut. Claude Code. Fresh chat with MCP.*

**The prompt** *(paste on screen):*

> ```
> Use Playwright MCP. Go to https://automationexercise.com/.
> Browse like a real shopper. Find a women's dress, add it to the cart,
> then tell me the cart total. Don't read the source — only use the browser.
> ```

**On camera:**

1. Paste. Send.
2. Agent navigates → snapshot → clicks Products → snapshot → filters Women → snapshot → picks a dress → "Add to cart" → modal → "View Cart" → reads total.
3. Agent reports: *"Added [dress name] for $XX. Cart total: $XX."*

> Watch the loop. The agent didn't know the site. It explored. Read the live state. Decided each click. Each step the page is **right there in its context** — that's the Brain working.
>
> Now imagine doing this with CLI. The agent would type a command, wait for a snapshot file to write to disk, `cat` the file, parse it, guess the next command, re-read the next file, parse again, decide again. **Possible — but clunky.** No live reasoning. Way more steps per click.
>
> **Exploration → MCP. Every time.**

---

### 3:30–4:30 — 2b: Where CLI wins (batch screenshots)

*Cut. Terminal.*

**Setup beat:**

```bash
npm install -g @playwright/cli
playwright-cli install --skills   # optional — teaches Claude the commands
```

> CLI installed. Now I'm researching CMS options for my blog. I want screenshots of five competitor homepages — straight to disk, no exploration needed.

**The prompt** *(paste on screen):*

> ```
> Use Playwright CLI. Take a full-page screenshot of each of these
> homepages and save them to /competitors as PNG files:
>
> - https://tina.io
> - https://strapi.io
> - https://www.sanity.io
> - https://www.contentful.com
> - https://decapcms.org
>
> No exploration. No reasoning. Just screenshot, save, move on.
> ```

**On camera:**

1. Paste. Send.
2. Agent loops: `playwright-cli goto` → `playwright-cli screenshot --full-page` → next site. Five times.
3. Show `/competitors/` folder. Five PNGs.

> The agent never "saw" any of these pages. **It didn't need to.** Five terminal commands. Files on disk. Done.
>
> If I'd done this with MCP — every screenshot would've come back as raw image bytes plus the accessibility snapshot, **straight into the chat context**. Five sites. **Way over a hundred thousand tokens.** For a job that needs zero reasoning. That's the wrong tool for the job.

*Cut to Microsoft's screenshot from the official Playwright CLI vs MCP video — token comparison panel.*

| Tool | Same task | Tokens |
|---|---|---|
| Playwright MCP | doc lookup + 4 screenshots | **~114,000** |
| Playwright CLI | doc lookup + 4 screenshots | **~27,000** |

> Microsoft's own benchmark. **About four times cheaper** on a batch task.

---

### 4:30–4:50 — The decision

*Camera. Quick beat.*

> So the rule. Per Microsoft's own docs:
>
> **CLI** is best for *coding agents working with large codebases* — Claude Code, Copilot, Cursor with file access. Token-efficient, headless, batch-friendly.
> **MCP** is best for *specialized agentic loops with persistent state and iterative reasoning over page structure* — exploring, browsing as a user, debugging interactively.
>
> One more thing — **CLI needs a coding agent** because it reads files from disk. If you're using Claude Desktop, or any generic MCP client without filesystem access — **MCP is your only option**.
>
> Both can coexist. Pick the right one per task.

---

## 4:50–5:10 — There's also: three test agents

*Cut. Camera.*

> Quick mention before we move on. Playwright also ships **three test-specific AI agents** — bundled in their own MCP server called Playwright Test MCP.

| Agent | Job |
|---|---|
| **Planner** | Reads your live app, writes a Markdown test plan |
| **Generator** | Plan → real Playwright TypeScript. Verifies selectors live. |
| **Healer** | Failing test → finds broken locator → patches → re-runs. |

> One command:

```bash
npx playwright init-agents --loop=claude
```

> TypeScript only. Playwright 1.56 plus.
>
> I'm not running these full demos here — **Debbie O'Brien** from the Playwright team has a brilliant deep-dive on her channel. Linked below. **Net result on her movies app — 75 percent of 107 tests written by these agents.**

---

## 5:10–6:10 — Vibe-coding callback: my Tina blog

*Cut to my blog repo. Tina CMS visible. Side-by-side Claude Design mockup.*

> Last one. **Playwright isn't just for testing anymore.**
>
> If you've watched my previous video — the 20-minute Tina CMS setup — you know I'm building my personal blog on Tina. Markdown content, Next.js, file-based, fully editable in browser. Link in description if you missed it.
>
> I'm not a frontend designer. So for the past couple months I've been improving the UI with **Claude Code + Claude Design** — and Playwright is the feedback loop that makes it actually work.

*Screen: Claude Code agent writes a component → calls Playwright (CLI by default for cost, MCP when it needs to explore) → opens browser headless → reads accessibility tree → patches CSS → reruns.*

> Watch the loop. The agent isn't using vision. It's reading the **accessibility tree** — every heading, every landmark, role, name. Same way a screen reader sees the page. Structured. Deterministic.
>
> Missing alt text? It knows. Layout broken on mobile? It knows. API call failing? It pulls the network log. Console error? It reads the console.
>
> **Playwright is eyes, ears, and dev tools — all in one. That's what I've handed to my coding agent.**
>
> Same loop works for any frontend you're building. Forms, login flows, browser automation, debugging APIs — anywhere you'd touch a browser by hand. **AI does it instead, and it actually knows what it's doing because it can see what it's doing.**

---

## 6:10–6:30 — CTA + resources + new SSW rule

*Camera. New SSW rule overlay. Resource list overlay.*

> If you want to go deeper:
> - **Playwright docs** — `playwright.dev` — best in class
> - **Debbie O'Brien's YouTube channel** — Playwright team, weekly deep-dives
> - **SSW Dojo** — our internal training, ask your consultant
>
> SSW just shipped a new rule for this — **"Do you know how to use Playwright with AI Agents?"** Pairs with our existing rule, *"Do you do automated UI testing?"*. Both cross-reference. Link in the description.
>
> Playwright isn't a test runner anymore. **It's how AI agents touch the web.**
>
> I'm Hark from SSW. **Go ship.**

---

## Delivery notes

- "**Playwright in the AI era**" — punch *AI*, beat after.
- "**Trust me.**" after the Node SDK line — half-smile, dead beat.
- "**The agent acted as a user.**" — slow, separate beats.
- "**Human in the loop.**" — punch each word.
- "**MCP is stateful but token-heavy. CLI is stateless but token-cheap.**" — slow, contrast.
- "**That's the Brain working.**" — flat, certain.
- "**Possible — but clunky.**" — flat dismissive.
- "**Way over a hundred thousand tokens.**" — punch *hundred thousand*.
- "**About four times cheaper**" — punch *cheaper*.
- "**Microsoft's own benchmark**" — flat, factual.
- "**Playwright is eyes, ears, and dev tools — all in one.**" — slow, beats per phrase.
- "**It actually knows what it's doing because it can see what it's doing.**" — slow, deliberate.
- "**It's how AI agents touch the web.**" — slow, then "**Go ship.**" — dry cut.

## Cut list if running long (in order — only if absolutely necessary)

1. Trim Demo 1 narration — show the agent working, less voiceover. Saves ~15s.
2. Trim agents section to one sentence: "Plus three test-specific agents — Planner, Generator, Healer. Link below." Saves ~15s.
3. Drop the SDK tip in 0:20–0:40 to one sentence. Saves ~5s.
4. Drop the resources list in CTA — just point at description. Saves ~10s.

## On-screen / B-roll shopping list

- `Playwright-CTF/tests/example.spec.ts` open in VS Code
- `pnpm test:headed` — Chromium pop, green
- UI mode trace viewer (flash)
- VS Code Playwright extension locator picker (flash)
- playwright.dev homepage — three boxes side by side
- Demo 1 prompt — paste-friendly on screen
- `claude mcp add playwright …` running, allow flow
- MCP browser tool calls — agent on tina.io, dismissing cookie banner, opening hamburger
- Generated `tests/tina.spec.ts` open
- `pnpm test:headed tests/tina.spec.ts` — Chromium runs, pass
- Brain vs Hard Drive comparison overlay
- Demo 2a prompt — paste-friendly
- automationexercise.com — agent shopping flow, cart total
- Demo 2b prompt — paste-friendly
- `npm install -g @playwright/cli` and `playwright-cli install --skills`
- CLI loop running — five `playwright-cli` calls, files appearing in `/competitors/`
- `/competitors/` folder open — five PNGs visible
- Microsoft's screenshot from CLI vs MCP video — token comparison panel
- Decision rule overlay — when to use which
- `npx playwright init-agents --loop=claude` output tree
- `.github/chat-modes/` showing planner.md / generator.md / healer.md
- Reference to Debbie O'Brien's channel
- Tina blog repo with side-by-side Claude Design mockup + live site
- Claude Code agent reading accessibility tree, patching CSS, rerunning
- Resources overlay — Playwright docs, Debbie's channel, SSW Dojo
- New SSW rule overlay — "Do you know how to use Playwright with AI Agents?"
- Cross-link overlay — existing "Do you do automated UI testing?" rule
- Callback chip — link to prior 20-min Tina CMS setup video

## Demo prompts (paste-ready)

**Demo 1 — AI writes the test on tina.io:**
```
I want to explore the home page of https://tina.io as a real user.
Use Playwright MCP. Open the page, dismiss any cookie banner if it
shows up, walk through the header, hamburger menu, and main CTAs.
Then suggest exactly ONE simple test scenario I can verify — something
like a navigation flow, a visible heading, or a working CTA.

When you're done, write the Playwright test for it. Save it to
tests/tina.spec.ts. Don't run it — I'll run it myself.
```

**Demo 2a — MCP wins (explorative shopping):**
```
Use Playwright MCP. Go to https://automationexercise.com/.
Browse like a real shopper. Find a women's dress, add it to the cart,
then tell me the cart total. Don't read the source — only use the browser.
```

**Demo 2b — CLI wins (batch screenshots):**
```
Use Playwright CLI. Take a full-page screenshot of each of these
homepages and save them to /competitors as PNG files:

- https://tina.io
- https://strapi.io
- https://www.sanity.io
- https://www.contentful.com
- https://decapcms.org

No exploration. No reasoning. Just screenshot, save, move on.
```

**Bonus prompt — if Demo 1 test passes and you want to show "ask AI to fix":**
```
The test in tests/tina.spec.ts is now failing because I changed something
in it. Use Playwright MCP to figure out why. Look at the live page. Then
fix the test. Don't change the assertion intent — keep what we're checking,
just fix the locator.
```

## Open follow-ups before recording

- [ ] Pre-test `tina.io` — confirm cookie banner dismissable, hamburger works on desktop viewport, `/docs` page loads
- [ ] Pre-test `automationexercise.com` — women's dress flow works, cart total displays
- [ ] Pre-test all 5 CMS homepages load without auth/Cloudflare/popup blocker
- [ ] Verify `playwright-cli --help` matches official docs
- [ ] Confirm new SSW rule URL exists by record day — overlay needs a real link
- [ ] Confirm cross-link is live on existing "Do you do automated UI testing?" rule
- [ ] Pre-bake Brain vs Hard Drive overlay
- [ ] Pre-bake decision rule overlay
- [ ] Pre-bake resources overlay (docs / Debbie / SSW Dojo)
- [ ] Capture Microsoft's CLI vs MCP screenshot (their video, ~0:55, side-by-side token panel)
- [ ] Sanity check: chromium installed (`pnpm exec playwright install chromium`)
- [ ] Demo machine: clean MCP state (`claude mcp list` → nothing playwright)
- [ ] Demo machine: `playwright-cli` not on PATH
- [ ] Confirm previous Tina blog setup video URL for callback overlay

## Sources / fact-check

| Claim | Source | Direct quote / number |
|---|---|---|
| MCP reads accessibility tree, not pixels | [playwright.dev/mcp/introduction](https://playwright.dev/mcp/introduction) | "operates on the accessibility tree, not pixels" |
| ~200–400 tokens per snapshot | [playwright.dev/mcp/introduction](https://playwright.dev/mcp/introduction) | "~200-400 tokens per snapshot vs thousands for DOM/screenshots" |
| 114k vs 27k tokens, ~4× | [Playwright CLI vs MCP video](https://www.youtube.com/watch?v=Be0ceKN81S8) (Microsoft) | "MCP… 114,000 tokens… CLI has taken only 26.8 thousand tokens" |
| CLI best for coding agents w/ large codebases | [playwright.dev/agent-cli/introduction](https://playwright.dev/agent-cli/introduction) | "best for coding agents (Claude Code, Copilot)" |
| MCP best for specialized agentic loops, persistent state, iterative reasoning | [playwright.dev/agent-cli/introduction](https://playwright.dev/agent-cli/introduction) | "specialized agentic loops that benefit from persistent state and iterative reasoning over page structure, such as exploratory automation or long-running autonomous workflows" |
| CLI requires Node 18+ and a coding agent w/ FS access | [playwright.dev/docs/getting-started-cli](https://playwright.dev/docs/getting-started-cli) | "Claude Code, GitHub Copilot… Node.js 18+" |
| CLI install: `npm install -g @playwright/cli` | [playwright.dev/agent-cli/introduction](https://playwright.dev/agent-cli/introduction) | exact command |
| Skills are optional | [playwright.dev/docs/getting-started-cli](https://playwright.dev/docs/getting-started-cli) | "discover commands on its own" without skills |
| Test agents — TS only, Playwright 1.56+ | [Playwright Testing Agents under the hood](https://www.youtube.com/watch?v=HLegcP8qxVY) | direct from transcript |
| `init-agents --loop=` flavors: vscode, claude, opencode | same video | direct from transcript |
| 75% of 107 tests agent-written | same video | direct from transcript |

**Things I am NOT claiming on camera:**

- Don't claim CLI is "always" cheaper — single-shot tasks are similar.
- Don't claim MCP is obsolete — Microsoft positions both as complementary.
- Don't claim CLI works without a coding agent.
- Don't claim "no vision ever" — MCP can take screenshots when asked. Default path is accessibility tree only.
- Don't claim `init-agents` works for Python/Java/.NET. TS only.
- **Don't mention scraping at all** — explicitly out of scope per Adam's call.
