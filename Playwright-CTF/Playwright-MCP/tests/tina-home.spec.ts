import { test, expect } from '@playwright/test';

test.describe('tina.io homepage', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('https://tina.io/');
    await page.getByRole('button', { name: 'Reject All' }).click();
  });

  test('has expected title', async ({ page }) => {
    await expect(page).toHaveTitle(/TinaCMS/);
  });

  test('hero shows Sites, Docs and Llamas tagline]', async ({ page }) => {
    await expect(
      page.getByRole('img', { name: /Docs and Llamas\.?\s*No Dramas/i })
    ).toBeVisible();
  });

  test('install snippet is visible', async ({ page }) => {
    await expect(page.getByText(/npx create-tina-app@latest/)).toBeVisible();
  });

  test('GitHub star link points to tinacms repo', async ({ page }) => {
    const gh = page.getByRole('link', { name: /us on GitHub/i });
    await expect(gh).toBeVisible();
    await expect(gh).toHaveAttribute('href', 'https://github.com/tinacms/tinacms');
  });

  test('footer credits SSW as maintainer', async ({ page }) => {
    const sswLink = page.getByRole('link', {
      name: /TinaCMS is maintained by.*Australia's leading software consultants/i,
    });
    await expect(sswLink).toBeVisible();
    await expect(sswLink).toHaveAttribute('href', 'https://www.ssw.com.au');
  });

  test('SSW appears in showcase', async ({ page }) => {
    const sswShowcase = page.locator('a[href="/showcase#ssw"]').first();
    await expect(sswShowcase).toBeVisible();
  });
});
