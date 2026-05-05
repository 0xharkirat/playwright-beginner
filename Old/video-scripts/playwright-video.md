# Playwright video — script v3

**Target length**: 5:10–5:25
**Tone**: conversational, host-to-camera, fast cuts to code
**Audience**: any dev or QA shipping a web UI

Read it out loud. If a sentence makes you pause weird, rewrite it. No semicolons. No colons mid-sentence. Contractions everywhere.

---

## 0:00–0:15 — Intro

*Camera. You, talking to the lens.*

> If your app doesn't have end-to-end tests, this is for you.
>
> I'm Hark. In the next four to five minutes I'll tell you when to use Playwright, when not to, and the major features. Plus AI, Playwright MCP servers, and CLI tools.
>
> Let's go.

---

## 0:15–1:10 — What even is Playwright

*Camera. Quick cut to VS Code with a tiny test file on screen.*

> Okay so for anyone still wondering, what is Playwright.
>
> Playwright is a testing framework.
>
> You've got unit tests, those check the smallest pieces of your code in isolation. One test for one function.
>
> You've got integration tests, those check the pieces talking to each other. Your code and the database. Your code and an API.
>
> And then there's the layer most of us skip. The UI. A real user clicking through a real browser. That's where Playwright comes in. End-to-end.
>
> Here's what a test looks like. Given the user is on the checkout page, when they click pay, a modal should open with their order summary.
>
> When you run it, Playwright fires up a real browser. Headless or on screen, your call. Finds the button. Clicks it. Checks for the modal. Modal opens, test passes. Doesn't, test fails.
>
> That's the high-level shape.

---

## 1:10–1:20 — The catch

*Camera.*

> Now here's the catch. End-to-end tests are slow. They're expensive to maintain. And they get flaky. So you've got to be smart about when you reach for this.

---

## 1:20–1:55 — When to use it, when not to

*Camera. Simple bullet list pops up next to you.*

> Use Playwright on the stuff that, if it breaks, your app is dead. Login. Checkout. Add to cart. The dashboard people open every morning.
>
> If checkout's broken, you don't have a shop. If add to cart's broken, same thing. Test that stuff. Hard.
>
> Don't reach for it because a button turned blue instead of red. Don't write a test for every little UI tweak. The more tests you pile on, the more flakiness you get in your pipeline. I've watched it happen. You'll waste more time chasing false failures than you save.
>
> Cover the critical paths. Skip the rest.

---

## 1:55–2:30 — Feature 1, multi-device

*Cut to VS Code. Show `playwright.config.ts` `projects` array. Then terminal running `npx playwright test`. Three browsers light up.*

> First feature. One test, every device.
>
> I write the test once. Playwright runs it on Chrome. Mobile Safari. iPad. All from one config file.
>
> Change one line, run everywhere. The app I work on runs against three browsers on every pull request. That's free coverage. You don't get that with anything else.

---

## 2:30–3:05 — Feature 2, network mocking

*Cut to VS Code. `page.route()` returning a fake 500. Switch to browser, error state on screen.*

> Second one. Network mocking.
>
> You intercept the API and return whatever you want. Want to see what your app does on a 500? Easy. Offline? Easy. A slow network? Easy.
>
> No backend needed. This is how you finally test those error states you've been ignoring for a year.

---

## 3:05–3:40 — Feature 3, multi-session

*Cut to VS Code. Two `browser.newContext()` calls side by side. Then split-screen test run.*

> Third feature. Two browsers, one test.
>
> Admin approves something on one side. User sees it pop up on the other side. Real multi-user flows, no mocking, no fake events.
>
> Anything with roles or permissions. Approvals. Chat. Live dashboards. This is the move.

---

## 3:40–4:35 — AI agents, MCP, and the CLI

*Terminal. Run `npx playwright init-agents --loop=vscode`. Cut to Planner running, then Generator writing a test, then Healer fixing one.*

> Now the fun part.
>
> Playwright ships with AI agents. There's a Planner that reads your app and writes a test plan. A Generator that turns the plan into real Playwright code. And a Healer that fixes your tests when the UI changes.
>
> You run one CLI command, `npx playwright init-agents`, and you're set up. Behind the scenes it's all MCP, the same protocol Copilot uses, so the agents can actually open a browser and look at your app while they work.
>
> I went from acceptance criteria to a passing test suite in one afternoon. That used to be two sprints of work. It's gone.

---

## 4:35–5:10 — Real-world proof

*Cut to a GitHub repo. Green Playwright job in Actions. Then a screen recording of the LinkedIn scraper printing names, blurred.*

> Two quick examples from real work.
>
> First, I added Playwright to a production app. UI breaks, build breaks, every single pull request. Devs hate it for one day and love it forever.
>
> Second, for fun, I scraped a bunch of LinkedIn bios with about thirty lines of Playwright. Just to see who actually follows their own company's profile convention. Let's just say, there's work to do.

---

## 5:10–5:25 — CTA

*Back to camera. Calm, direct.*

> Look. If you're shipping a UI without end-to-end tests, you're shipping on vibes.
>
> Playwright takes a day to set up and pays back forever. Link's in the description.
>
> Go play.

---

## Notes for delivery

- Pause on the line breaks. Each break is a real beat, not just formatting.
- "Easy. Easy. Easy." in the network section, hit each one harder than the last.
- "It's gone." after the AI section, dead beat then move on. Don't sell it.
- CTA line, "you're shipping on vibes" is the punchline. Land it dry, then half-smile.

## Cut list if running long

In order, drop:
1. Multi-session demo (saves ~35s)
2. LinkedIn scrape clip (saves ~15s)
3. Trim the intro to "I'll tell you when, when not, and the features. Plus AI. Let's go."
