# Playwright video — script v4 (AI era pivot)

**Target length**: 4:15–4:45
**Tone**: conversational, host-to-camera, fast cuts to terminal/VS Code
**Audience**: any dev or QA already shipping web UIs — not first-time Playwright

> **Scope change vs v3**: dropped the "what is Playwright / when not to / multi-device / network mocking / multi-session" tour. This one is *Playwright in the AI era*. MCPs, agents, CLI, when to use which.

Read it out loud. If a sentence makes you pause weird, rewrite it. No semicolons. No colons mid-sentence. Contractions everywhere.

---

## 0:00–0:15 — Intro

*Camera. You, talking to the lens. Quick energy.*

> Hey, I'm Hark from SSW. Today we're talking Playwright. But not just Playwright — Playwright in the AI era.
>
> Playwrith MCP servers, AI agents, A whole new CLI. And it changes how you test your frontends and as well as how you can ship better frontends. Let's jump in.

---

## 0:15–0:40 — Quick "what is Playwright" (for the new folks only)

*Camera. Cut briefly to playwright.dev landing.*

> Quick one for anyone new. Playwright isn't just a UI testing framework anymore—it’s the industry standard for web automation, testing, and now, AI agents. Built by Microsoft. Available for TypeScript, Python, .NET, and Java.

> One pro tip. The TypeScript and Node version is the absolute gold standard. The .NET port doesn't lack core features, but it doesn't have Playwright’s native test runner and developer tooling. If you're starting fresh, go Node. Trust me.

---

## 0:40–1:00 — Old way, fast

*Cut to VS Code. Tiny test file. Then `npx playwright test` running a browser.*

> Old workflow looked like this. Write a test. Run `npx playwright test`. Real browser opens. Click. Assert. Pass or fail. Done.
>
> Still works. Still great. But that's not the whole story anymore.

---

## 1:00–1:15 — The pivot

*Cut to playwright.dev homepage. Three boxes side by side. Highlight each.*

> Go to playwright.dev today. Three sections side by side. Playwright Test. Playwright MCP. Playwright CLI. And there's a hidden fourth — Playwright Test MCP. Yeah. It's confusing. Let me sort it.

---

## 1:15–2:15 — Playwright Test MCP + the three agents

*Cut to terminal. Run `npx playwright init-agents --loop=vscode`. Show the generated `.github/`, `specs/`, `tests/seed.spec.ts`. Then VS Code chat picker showing Planner / Generator / Healer.*

> First, the testing side. Playwright Test now ships with three AI agents.
>
> **Planner.** Reads your app live, writes a test plan in Markdown.
> **Generator.** Turns that plan into real Playwright code, verifies selectors as it writes.
> **Healer.** Runs failing tests, finds the broken locator, patches it, re-runs. Autonomous.
>
> One command sets it all up. `npx playwright init-agents --loop=vscode`. Or `--loop=claude`. Or `--loop=opencode`. It drops agent definitions into `.github`, generates a seed spec, and quietly spins up a private MCP server called Playwright Test MCP. Local to your repo only.
>
> Catch. TypeScript only, for now.
>
> For proof — Debbie O'Brien put 107 tests on her movies app. About 75% written by these agents. That's not vibes. That's merged code.

---

## 2:15–3:00 — Playwright MCP (the standalone one)

*Cut to VS Code MCP config. `@playwright/mcp` running. Then a chat: "navigate to ssw.com.au, tell me what it's about" — agent calls tools.*

> Second. The standalone Playwright MCP server. Different package. `npx @playwright/mcp@latest`.
>
> This one isn't just for tests. It hands your AI agent a browser. Click, type, navigate, fill forms, scrape — anything you do in Chrome, the agent can do.
>
> The clever bit. It doesn't send screenshots back to the LLM. It sends the **accessibility tree**. Structured, named, deterministic. No vision model. No hallucinated click coords.
>
> Use it for vibe-coding a frontend with a real feedback loop. Agent writes a component, opens the browser headless, checks it actually rendered the right thing. That's huge.

---

## 3:00–3:25 — Two MCPs, when each one fires

*Camera. Simple two-column overlay: "Test MCP" vs "Playwright MCP".*

> So two MCPs. Don't mix them up.
>
> **Playwright Test MCP** — bundled, per-project, only wakes up when you run the Planner, Generator, or Healer. Testing only.
> **Playwright MCP** — standalone, install once, works in any MCP client. General browser automation.
>
> Both can coexist. Pick the one that fits the job.

---

## 3:25–4:15 — Playwright CLI (the new kid)

*Cut to terminal. `npm install -g @playwright/cli@latest`. Then `playwright-cli install --skills`. Show a side-by-side: same task, MCP token cost vs CLI token cost.*

> Now the new one. Playwright CLI. Released this year.
>
> Here's the problem MCP has. Every page snapshot the agent looks at gets pumped back into the LLM's context. Microsoft did the math — same task, MCP burned a hundred and fourteen thousand tokens. Same task with the CLI, a fraction of that.
>
> CLI writes results to disk. The coding agent decides what to read. Headless by default. Way cheaper. Way faster.
>
> Install. `npm install -g @playwright/cli@latest`. Then `playwright-cli install --skills` and your coding agent picks up Playwright knowledge automatically.
>
> Catch. CLI only works inside a coding agent that can read files — Claude Code, GitHub Copilot, that kind of setup. That's how it stays cheap.

---

## 4:15–4:45 — Decision matrix and CTA

*Camera. Three quick text overlays one at a time.*

> So which one do you reach for.
>
> Writing tests with AI? **Playwright Test MCP** plus the three agents.
> Driving a browser from any MCP client, or scraping, or vibe-coding a frontend? **Playwright MCP**.
> Coding inside Claude Code or Copilot and you care about tokens? **Playwright CLI**.
>
> Playwright isn't a test runner anymore. It's how AI agents touch the web.
>
> I'm Hark from SSW. Link's in the description. Go ship.

---

## Notes for delivery

- "Trust me." after the Node SDK line — half-smile, dead beat.
- The "Yeah. It's confusing." line — flat, a little tired. You're the guide.
- "**Autonomous.**" — punch the word, then beat.
- "That's not vibes. That's merged code." — separate beat for each sentence.
- "No vision model. No hallucinated click coords." — drop one, drop two. Same rhythm as v3's "Easy. Easy. Easy."
- The token comparison — read "**a hundred and fourteen thousand tokens**" slow, then "**a fraction of that**" quick. Contrast.
- CTA "Go ship." — dry, no smile, cut.

## Cut list if running long

In order:
1. Drop the "old way" beat at 0:40–1:00 (saves ~20s) — assume audience knows Playwright already.
2. Trim the Debbie 107-tests proof point (saves ~10s).
3. Compress the decision matrix to two lines instead of three.

## On-screen / B-roll shopping list

- playwright.dev homepage with the three boxes visible
- terminal: `npx playwright init-agents --loop=vscode` running clean
- VS Code chat picker showing Planner / Generator / Healer agents
- VS Code MCP config JSON with `@playwright/mcp` entry
- terminal: `npx @playwright/mcp@latest` doing a navigate + accessibility snapshot
- terminal: `npm install -g @playwright/cli@latest` then `playwright-cli install --skills`
- token comparison graphic (114k vs CLI fraction) — make this in Figma, 2-bar chart
- side-by-side: "Test MCP per-project" vs "Playwright MCP global"
