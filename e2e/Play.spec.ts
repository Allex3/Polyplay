import { test, expect } from '@playwright/test'

test('Starts correctly', async ({ page }) => {
  await page.goto('/games')

})
