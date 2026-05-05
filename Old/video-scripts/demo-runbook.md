# Demo runbook — record-ready

Each section = one take. Commands you type live on camera. Pitfalls flagged.

**Pre-flight (do once, off-camera):**
- Close all other terminals/apps. Clean dock.
- Demo machine in light/dark mode of your choice — pick one and stick with it.
- Browser zoom 110% in VS Code for legibility.
- Hide secrets in `~/.zshrc` aliases — no `OPENAI_API_KEY=` flashes.
- Have `Playwright-CTF/` open in VS Code. Have a fresh empty terminal.
- Verify `ssw.com.au/rules` loads in Chrome.
- Have Claude Code logged in.
- Have a fresh chat ready in Claude Code.
- **Reset state**: `cd /Users/hark/ssw/playwright-beginner/Playwright-CTF && rm -rf playwright-report test-results` before recording so the green-pass shot is clean.

---

## Take 1 — Old way (Playwright-CTF)

**On camera:**

1. Open VS Code → `Playwright-CTF/tests/example.spec.ts` (already open)
2. Show file. Two tests. Default scaffold.
3. Open terminal in VS Code split.
4. Run:
   ```bash
   pnpm test:headed
   ```
5. **Expect**: Chromium pops, runs `has title`, `get started link`, both pass. Terminal green: `2 passed`.
6. Edit line 7: `/Playwright/` → `/Hoaxing/`. Save.
7. Run:
   ```bash
   pnpm test:headed
   ```
8. **Expect**: Browser pops. `has title` fails. Terminal red. Locator + diff visible.
9. Revert. Save.
10. Quick montage (don't run full):
    ```bash
    pnpm test:ui      # show it open, hover trace, close
    pnpm test:debug   # show inspector pop, close
    ```
11. Click on element in VS Code Playwright extension panel → show locator picker overlay → close.

**Pitfalls:**
- If chromium binary missing: `pnpm exec playwright install chromium` first (off-camera).
- `pnpm test:ui` and `:debug` need `Ctrl+C` to exit cleanly. Don't leave hanging.
- `test-results/` and `playwright-report/` will write — delete after each take to keep VS Code file tree clean for next take.

---

## Take 2 — Install Playwright MCP live

**On camera:**

1. Open Claude Code.
2. **Say**: "Going to install Playwright MCP. Two ways — CLI command, or just ask Claude to do it."
3. **Option A (preferred — looks magical)**: prompt Claude:
   > "Install the Playwright MCP server for me. Use `npx @playwright/mcp@latest`. Set it up at user scope."

   Claude will run: `claude mcp add playwright -s user -- npx -y @playwright/mcp@latest`
4. **Option B (back-up if Claude waffles)**: type directly:
   ```bash
   claude mcp add playwright -s user -- npx -y @playwright/mcp@latest
   ```
5. Verify:
   ```bash
   claude mcp list
   ```
   **Expect**: `playwright: npx -y @playwright/mcp@latest - ✓ Connected`
6. **Say**: "Done. Agent has a browser now."

**Pitfalls:**
- First MCP tool call will trigger a permission prompt — pre-allow in this session OR show the allow flow on camera (good teaching moment).
- `-s user` makes it global. Use `-s project` if you want it scoped to this repo only.
- If MCP install hangs: `pkill -f mcp` and retry.

---

## Take 3 — MCP demo: agent as user on SSW Rules

**On camera:**

1. New chat in Claude Code.
2. Prompt:
   > "Open `https://www.ssw.com.au/rules/` using the Playwright MCP. Browse like a user. Explore the home page. Suggest one test scenario I can verify pass and fail. Don't read source — only use the browser."
3. Watch agent call `browser_navigate`, `browser_snapshot`, `browser_click`, etc.
4. Agent suggests scenario (e.g., "search for 'clean code' should return rule cards" or "click 'Top categories' shows category list").
5. Then prompt:
   > "Now write a Playwright test for that scenario. Save it to `Playwright-CTF/tests/rules.spec.ts`. Use `@playwright/test`."
6. Show the generated file.
7. Run:
   ```bash
   cd Playwright-CTF && pnpm test:headed tests/rules.spec.ts
   ```
8. **Expect**: pass.

**Pitfalls:**
- Rules site may have a cookie banner — agent might trip on it. If so, prompt: "Dismiss any cookie banner first."
- ssw.com.au is behind Cloudflare — if a challenge page appears, abort and try again. Pre-warm by visiting in real browser first.
- Test selectors may be flaky on first try. Have backup: ask agent to add `await page.waitForLoadState('networkidle')`.
- **Watch for**: agent inventing rules content. It should describe what it actually saw.

---

## Take 4 — Install + demo 3 agents (Planner / Generator / Healer)

**Setup beforehand (off-camera)**:
- Make a fresh demo folder: `mkdir -p ~/ssw/playwright-agents-demo && cd $_`
- `pnpm init -y` and `pnpm add -D @playwright/test@latest`
- `pnpm exec playwright install chromium`
- Optional: scaffold a tiny test target. **Or** point at `ssw.com.au/rules` — public, no setup.

**On camera:**

1. **Say**: "Three agents. One command."
2. Run:
   ```bash
   npx playwright init-agents --loop=claude
   ```
3. Show the generated tree:
   - `.github/chat-modes/planner.md`
   - `.github/chat-modes/generator.md`
   - `.github/chat-modes/healer.md`
   - `.vscode/mcp.json` (or `.mcp.json` for Claude)
   - `tests/seed.spec.ts`
4. Open `seed.spec.ts`. **Say**: "This runs before every generated test. For now, leave it minimal — just `await page.goto('https://www.ssw.com.au/rules/');`"
5. Edit seed → save.

**Planner take (~25s):**

6. New Claude Code chat → switch agent to `playwright-planner`.
7. Drag `tests/seed.spec.ts` into context.
8. Prompt:
   > "Generate a comprehensive test plan for the SSW Rules home page. Save it as `specs/rules-plan.md`."
9. Allow MCP tool calls. Agent browses live.
10. Show `specs/rules-plan.md` open. Markdown plan with sections.
11. **Commit immediately**:
    ```bash
    git add . && git commit -m "feat: planner output"
    ```

**Generator take (~25s):**

12. New chat → switch agent to `playwright-generator`.
13. Plan already in context.
14. Prompt:
    > "Generate one test for scenario 1 of the plan."
15. Watch tool calls: `setup_generator_page` → `verify_element_visible` → `retrieve_test_log` → `write_test`.
16. Show new file in `tests/`.
17. Run:
    ```bash
    pnpm exec playwright test --headed
    ```
18. **Expect**: pass.

**Healer take (~25s):**

19. Manually break a locator in the generated test. Save.
20. Run test → red.
21. New chat → switch to `playwright-healer`.
22. Prompt:
    > "Run and fix failing tests."
23. Watch: `debug_single_test` → page snapshot → identifies issue → patches → re-runs.
24. Show the test pass green.
25. `git diff` → highlight the patch.

**Pitfalls:**
- Need VS Code 1.105+ if using `--loop=vscode`. For `--loop=claude` need Claude Code current.
- Need Playwright **1.56+**. Check with `pnpm exec playwright --version`.
- Agents won't appear in chat picker until you reload the window (VS Code: `Cmd+Shift+P` → "Reload Window"; Claude Code: restart).
- `init-agents` creates a Playwright Test MCP entry in `mcp.json`. Don't confuse with the standalone `@playwright/mcp` from Take 2 — they're different MCP servers.
- Healer can be slow on big test suites — for video, point it at one failing file: drag the file into chat first.
- Always commit between agent runs. Healer's edits cascade — `git diff` is your only undo.

---

## Take 5 — Install + demo Playwright CLI

**On camera:**

1. **Say**: "MCP is great. But it burns tokens. Watch this."
2. Run:
   ```bash
   npm i -g @playwright/cli@latest
   ```
3. Run:
   ```bash
   playwright-cli install
   ```
4. Run:
   ```bash
   playwright-cli install --skills
   ```
5. **Say**: "Skills teach Claude Code Playwright knowledge. Now ask Claude to do something with the browser."
6. New Claude Code chat. Prompt:
   > "Use the Playwright CLI to open `https://www.ssw.com.au/rules/`, take a screenshot, and save it to `screenshots/rules.png`."
7. Watch agent call CLI commands. Show `screenshots/rules.png`.
8. **Pull up token comparison** (pre-baked Figma graphic): MCP **114k** vs CLI **26.8k**.

**Pitfalls:**
- Verify exact CLI command names live before recording. The skill install may have changed since Debbie's video. If `playwright-cli install --skills` errors, try:
  ```bash
  playwright-cli --help
  ```
  and read the actual subcommand.
- If global `npm i` fails on permissions: prefix with `sudo` (don't show this on camera — re-record).
- CLI is headless by default — terminal won't show a browser. That's the point. Mention it on camera.

---

## Take 6 — Vibe-coding UI w/ Playwright dashboard

**Setup beforehand**:
- Have a small frontend project (your personal site repo).
- Have a Claude Design mockup screenshot ready.

**On camera:**

1. New Claude Code chat in the site repo.
2. Drop in mockup screenshot. Prompt:
   > "Build the hero section to match this design. Use Playwright CLI to screenshot your work and compare. Iterate until it matches."
3. Watch agent loop: write code → `playwright-cli screenshot` → compare → patch.
4. Run:
   ```bash
   playwright-cli show
   ```
5. Dashboard opens. Show the running browser session. Scribble feedback on the screenshot.
6. Submit. Agent acts on it.

**Pitfalls:**
- Dashboard URL/port can vary. Have it pre-tested.
- If agent burns into a loop, interrupt and steer manually. Don't show it spiraling.

---

## Take 7 — Bonus: scraping (optional — record last, decide later)

**On camera (disclaimer first, dead serious):**

1. **Say**: "Last one. Controversial. Most sites' ToS forbid scraping. Use APIs when offered. Personal, non-commercial only. Respect ToS."
2. Show `scrape-linkedin.spec.ts` (pre-written, pre-run).
3. Walk through:
   - First run: log in once → `auth.json` saved
   - Subsequent runs: reuse storage state
   - Loop bios → JSON output
4. Show Claude-built dashboard.

**Pitfalls:**
- **Don't show real LinkedIn login on camera**. Show only the storage-state pattern in code, and mock JSON output. Real login = security risk + ToS risk.
- Mask all real names except your own.
- Decide post-edit whether to ship this section. Easy to cut.

---

## End-of-session cleanup

After all takes:

```bash
cd /Users/hark/ssw/playwright-beginner/Playwright-CTF
rm -rf test-results playwright-report
git status   # confirm only intentional changes
```

Off-camera tasks:
- Export raw recordings to project drive
- Build the **Figma token-comparison graphic** for Take 5
- Build the **decision matrix overlay** for outro

---

## Order of takes (recommended)

Record in this order — easiest to hardest, lowest to highest blast radius:

1. **Take 1** (old way) — 100% repeatable, no LLM
2. **Take 2** (MCP install) — quick, deterministic
3. **Take 5** (CLI install) — quick, deterministic
4. **Take 3** (MCP agent demo) — LLM, may need re-takes
5. **Take 4** (3 agents) — LLM + setup-heavy, likely re-takes
6. **Take 6** (vibe-coding) — LLM, hardest to guarantee
7. **Take 7** (scraping bonus) — pre-record, optional

If running tight: cut Take 6 first, then Take 7. Both are "bonus" tier.

---

## Pre-record final checklist

- [ ] `Playwright-CTF` chromium installed
- [ ] `claude mcp list` shows nothing playwright-related (so install demo is clean)
- [ ] `playwright-cli` not on PATH (so install demo is clean)
- [ ] Demo agents folder fresh (no `.github/chat-modes/` yet)
- [ ] `ssw.com.au/rules` loads w/o cookie banner blocker (real browser test)
- [ ] Claude Code logged in, fresh chat
- [ ] Recording software armed (OBS/ScreenStudio)
- [ ] Mic level checked
- [ ] All other terminals/apps closed
- [ ] Notifications muted (Do Not Disturb on)
