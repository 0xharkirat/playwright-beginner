# Playwright video — script v8 (Brain vs Hard Drive)

**Target length**: ~5 min hard cap (delivered fast)
**Tied to rule**: *Do you know how to use Playwright with AI Agents?* (new SSW rule, references existing *Do you do automated UI testing?*)
**Tone**: conversational, host-to-camera, short sentences, fast cuts to terminal/VS Code
**Audience**: SSW devs + wider dev/QA crowd
**Mental model**: **Brain (MCP) vs Hard Drive (CLI)**

> **Scope changes vs v7**:
> - Replaced "single MCP demo + brief CLI mention" with **two purpose-built demos** that show MCP vs CLI doing what each is *best at*.
> - Demo 1 (MCP wins): exploratory e-commerce flow on `automationexercise.com`.
> - Demo 2 (CLI wins): batch CMS homepage screenshots — also serves as **research for the Tina blog**, threading into vibe-coding callback.
> - 3 agents kept as a brief "and there's also this" mention.
> - Tina blog vibe-coding remains the closer.

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
> **Tip**. Node SDK is the gold standard. Other SDKs lag on tooling — no native test runner. If you're starting fresh, **go Node**. Trust me.

---

## 0:40–1:10 — Old way, fast

*VS Code. `Playwright-CTF/tests/example.spec.ts` open.*

> Old way. One command — `npm create playwright`. Hit defaults. Get a default test. Open browser, navigate, assert title.

```bash
pnpm test:headed
```

> Real Chromium. Real click. Pass.
>
> You also get **UI mode** with a trace viewer, **debug mode** with the Inspector, and the VS Code extension to pick locators visually.
>
> Still works. Still useful. **But you wrote every selector yourself.** Now watch.

---

## 1:10–1:25 — The mental model: Brain vs Hard Drive

*Camera. Two-column overlay.*

> playwright.dev today has three doors — **Playwright Test**, **Playwright MCP**, **Playwright CLI**. Plus a hidden fourth — **Playwright Test MCP** with the test agents.
>
> Here's the mental model that makes it click. **MCP is the Brain. CLI is the Hard Drive.**

| | MCP — "Brain" | CLI — "Hard Drive" |
|---|---|---|
| Where browser state lives | LLM context window | Local filesystem |
| Each click | New page snapshot streamed straight into AI's context | Result saved to a file. AI gets "Saved to snapshot.txt" |
| AI's view | Live, reactive, sees everything | Empty until it chooses to `cat` a file |
| Token cost | High — AI absorbs every page | Tiny — AI reads only what it needs |
| Best for | **Exploring**, dynamic flows, reasoning step-by-step | **Batch jobs**, repetitive tasks, generating code |

> Same browser underneath. Same Chromium. The difference is **where the data goes**.
>
> Let me show you each one doing what it's best at.

---

## 1:25–2:50 — Demo 1: Where MCP wins — "The Explorer"

*Claude Code. Empty chat. Terminal visible.*

**Setup beat (one line):**

```bash
claude mcp add playwright -s user -- npx -y @playwright/mcp@latest
```

> MCP installed. Now the prompt.

**The prompt** *(paste on screen):*

> ```
> Go to https://automationexercise.com/. Browse like a real shopper.
> Find a women's dress, add it to the cart, then tell me the cart total.
> Don't read the source. Only use the browser.
> ```

**On camera:**

1. Paste prompt. Send.
2. Agent calls `browser_navigate("https://automationexercise.com/")`.
3. Snapshot streams back — agent sees the homepage, "Women" category, "Products" link.
4. Agent clicks through Products → Women → picks a dress → "Add to cart" → cart modal → "View Cart" → reads cart total.
5. Agent reports back: *"Added [dress name]. Cart total: $XX."*

> Watch that loop. **The agent didn't know the site.** It explored. Read the live state. Decided the next click. Then the next. Each step the page is **right there in its context** — that's the Brain.
>
> This is exactly what MCP is built for. **Exploratory, dynamic, stateful.**

**Side note — token cost:**

*Open `/context` indicator briefly.*

> Quick look at context — that flow burned around **30 to 40 thousand tokens** on a short session. Bigger sites, more steps — climbs fast. Worth it for exploration. Not worth it for batch work. Which brings me to demo two.

---

## 2:50–3:50 — Demo 2: Where CLI wins — "The Workhorse"

*Cut. Terminal.*

**Setup beat:**

```bash
npm install -g @playwright/cli
playwright-cli install --skills   # optional — teaches Claude the commands
```

> CLI installed. Now I'm researching CMS options for my blog. I want screenshots of five competitor homepages.

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

1. Paste prompt. Send.
2. Agent loops `playwright-cli goto` + `playwright-cli screenshot --full-page` per site.
3. Files land in `/competitors/`. Show the folder. Five PNGs.

> Watch the difference. The agent didn't "see" any of these pages. **It didn't need to.** It typed five commands. CLI did the work. Files on disk.
>
> If I'd asked MCP to do this — every screenshot would've come back as raw image bytes into the context window. **Five sites. Way over a hundred thousand tokens.** For a job that needs zero reasoning.

*Cut to Microsoft's own screenshot — token comparison from the official Playwright CLI vs MCP video.*

| Tool | Same task | Tokens |
|---|---|---|
| Playwright MCP | doc lookup + 4 screenshots | **~114,000** |
| Playwright CLI | doc lookup + 4 screenshots | **~27,000** |

> Microsoft's own benchmark. **About four times cheaper** on a batch task.

---

## 3:50–4:10 — The decision

*Camera. Quick beat.*

> So the rule is simple.
>
> **Need the AI to explore, reason, react step-by-step?** → **MCP.** The Brain.
> **Need batch jobs, repetitive work, code generation, screenshots, scraping a list?** → **CLI.** The Hard Drive.
>
> One more nuance — **CLI needs a coding agent** like Claude Code or Copilot, because the agent has to read files. If you're using Claude Desktop, Cursor without file access, or any other generic MCP client — MCP is your only option.
>
> Both can coexist. Pick the right one per task.

---

## 4:10–4:30 — There's also: Playwright Test MCP + 3 agents

*Cut. Camera.*

> One more piece — Playwright also ships **three test-specific AI agents**.

| Agent | Job |
|---|---|
| **Planner** | Reads your live app, writes a Markdown test plan |
| **Generator** | Plan → real Playwright TypeScript. Verifies selectors live. |
| **Healer** | Failing test → finds broken locator → patches → re-runs. |

```bash
npx playwright init-agents --loop=claude
```

> Drops agent definitions into `.github/chat-modes/`. **TypeScript only**, Playwright 1.56 plus.
>
> Debbie O'Brien from the Playwright team has a deep-dive video — link below. **Net result on her movies app: 75 percent of 107 tests written by these agents.** Worth a watch.

---

## 4:30–4:50 — Beyond testing: my Tina blog feedback loop

*Cut to my blog repo. Tina CMS visible. Side-by-side Claude Design mockup.*

> Last one. **Playwright isn't just for testing anymore.**
>
> Callback to Demo 2 — those CMS screenshots? That was for my blog. I'm building it on **Tina CMS**. Markdown, Next.js, file-based. I'm not a frontend designer. So I gave Claude a design and said: **build it, then use Playwright to check your own work.**

*Screen: Claude Code agent writes a component → calls Playwright (CLI for cost, MCP if it needs to explore) → opens browser headless → reads accessibility tree → patches CSS → reruns.*

> The agent isn't using vision. It's reading the **accessibility tree** — every heading, every landmark, role and name. Same way a screen reader sees the page. Structured. Deterministic.
>
> Missing alt text? It knows. Layout broken? It knows. API failing? It pulls the network log. Console error? It reads the console.
>
> **Playwright gives the agent eyes, ears, and dev tools, all in one.**

---

## 4:50–5:00 — CTA + new SSW rule

*Camera. New SSW rule overlay.*

> SSW just shipped a new rule for this — **"Do you know how to use Playwright with AI Agents?"** Pairs with our existing rule, *"Do you do automated UI testing?"*. Both cross-reference. Link below.
>
> Playwright isn't a test runner anymore. **It's how AI agents touch the web.**
>
> I'm Hark from SSW. **Go ship.**

---

## Delivery notes

- "**Playwright in the AI era**" — punch *AI*, beat after.
- "**Trust me.**" after the Node SDK line — half-smile, dead beat.
- "**MCP is the Brain. CLI is the Hard Drive.**" — slow, separate beats. Repeat the metaphor.
- "**The agent didn't know the site. It explored.**" — slow.
- "**It didn't need to.**" — flat, certain.
- "**Five sites. Way over a hundred thousand tokens.**" — punch *hundred thousand*.
- "**About four times cheaper**" — punch *cheaper*.
- "**Microsoft's own benchmark**" — flat, factual.
- "**Playwright gives the agent eyes, ears, and dev tools, all in one.**" — slow, beats per phrase.
- "**It's how AI agents touch the web.**" — slow, then "**Go ship.**" — dry cut.

## Cut list if running long (in order)

1. Trim Demo 1 narration — show the agent working, less voiceover. Saves ~15s.
2. Drop the live `/context` token peek in Demo 1 — Microsoft's screenshot in Demo 2 carries the point. Saves ~10s.
3. Trim agents section to one sentence: "Plus three test-specific agents — Planner, Generator, Healer. Link below." Saves ~15s.
4. Drop the SDK tip in 0:20–0:40 to one sentence. Saves ~5s.

## On-screen / B-roll shopping list

- `Playwright-CTF/tests/example.spec.ts` open
- `pnpm test:headed` — Chromium pop, green
- playwright.dev homepage — three boxes side by side
- Brain vs Hard Drive comparison overlay
- `claude mcp add playwright …` running, allow flow
- The Demo 1 prompt — paste-friendly on screen
- MCP browser tool calls — `browser_navigate`, `browser_snapshot`, `browser_click` — agent shopping
- automationexercise.com — agent finding a dress, cart total
- `/context` indicator showing token usage
- `npm install -g @playwright/cli` running
- The Demo 2 prompt — paste-friendly on screen
- CLI loop running — five `playwright-cli` calls, files appearing in `/competitors/`
- `/competitors/` folder open — five PNGs visible
- Microsoft's screenshot from CLI vs MCP video — token comparison (114k vs 27k)
- Decision matrix overlay — Brain vs Hard Drive use cases
- `npx playwright init-agents --loop=claude` output tree
- `.github/chat-modes/` showing planner.md / generator.md / healer.md
- Blog repo with Tina CMS — Claude Design mockup side by side with live site
- Claude Code agent reading accessibility tree, patching CSS, rerunning
- New SSW rule overlay
- Cross-link overlay to existing automated UI testing rule

## Demo prompts (paste-ready)

**Demo 1 — MCP Explorer:**
```
Go to https://automationexercise.com/. Browse like a real shopper.
Find a women's dress, add it to the cart, then tell me the cart total.
Don't read the source. Only use the browser.
```

**Demo 2 — CLI Workhorse:**
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
The test in tests/<file>.spec.ts is now failing because I changed something
in it. Use Playwright MCP to figure out why. Look at the live page. Then
fix the test. Don't change the assertion intent — keep what we're checking,
just fix the locator.
```

## Open follow-ups before recording

- [ ] Pre-test `automationexercise.com` — confirm no cookie banner blocker, women's dresses category exists, cart flow works
- [ ] Pre-test all 5 CMS homepages load without auth/Cloudflare challenge
- [ ] Verify `playwright-cli --help` matches official docs
- [ ] Confirm new SSW rule URL exists by record day — overlay needs a real link
- [ ] Confirm cross-link is live on existing "Do you do automated UI testing?" rule
- [ ] Pre-bake Brain vs Hard Drive comparison overlay
- [ ] Pre-bake decision matrix overlay
- [ ] Capture Microsoft's CLI vs MCP screenshot (their video, ~0:55 timestamp, side-by-side token panel)
- [ ] Sanity check: chromium installed (`pnpm exec playwright install chromium`)
- [ ] Demo machine: clean MCP state (`claude mcp list` → nothing playwright)
- [ ] Demo machine: `playwright-cli` not on PATH

## Sources / fact-check

| Claim | Source | Direct quote / number |
|---|---|---|
| MCP reads accessibility tree, not pixels | [playwright.dev/mcp/introduction](https://playwright.dev/mcp/introduction) | "operates on the accessibility tree, not pixels" |
| ~200–400 tokens per snapshot | [playwright.dev/mcp/introduction](https://playwright.dev/mcp/introduction) | "~200-400 tokens per snapshot vs thousands for DOM/screenshots" |
| 114k vs 27k tokens, ~4× | [Playwright CLI vs MCP video](https://www.youtube.com/watch?v=Be0ceKN81S8) (Microsoft) | "MCP… 114,000 tokens… CLI has taken only 26.8 thousand tokens" |
| CLI best for coding agents | [playwright.dev/agent-cli/introduction](https://playwright.dev/agent-cli/introduction) | "best for coding agents (Claude Code, Copilot)" |
| MCP best for specialized agentic loops, persistent state, iterative reasoning | [playwright.dev/agent-cli/introduction](https://playwright.dev/agent-cli/introduction) | "specialized agentic loops that benefit from persistent state and iterative reasoning over page structure, such as exploratory automation or long-running autonomous workflows" |
| CLI requires Node 18+ and a coding agent w/ FS access | [playwright.dev/docs/getting-started-cli](https://playwright.dev/docs/getting-started-cli) | "Claude Code, GitHub Copilot… Node.js 18+" |
| CLI install: `npm install -g @playwright/cli` | [playwright.dev/agent-cli/introduction](https://playwright.dev/agent-cli/introduction) | exact command from docs |
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
