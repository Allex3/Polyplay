import { test, expect } from '@playwright/test'

test('Starts correctly', async ({ page }) => {
  await page.goto('/user')
})

test('add new game', async ({ page }) => {
  await page.goto('/register')

  await page.locator('#inputUsername').fill('alex')
  await page.locator('#inputEmail').fill('email@example.com')
  await page.locator('#inputPassword').fill('alex.2005')
  await page.locator('#register').click()

  await page.waitForSelector('#login')

  await page.locator('#inputUsername').fill('alex')
  await page.locator('#inputPassword').fill('alex.2005')
  await page.locator('#login').click()

  await page.waitForSelector('#openAddGameModal')

  await page.locator('#openAddGameModal').click()

  await page.locator('#addGameNameField').fill('')
  await page.locator('#addGameDescriptionField').fill('')
  await page.locator('#addGameTagField').fill('Incremental')
  await page.locator('#addGameImageField').fill('../../assets/logo.png')

  await page.locator('#addGameButton').click()

  await expect(page.locator('#addGameError')).toContainText('empty')

  //await page.locator('#addGameNameField').fill('######')
  //await page.locator('#addGameButton').click()

  //await expect(page.locator('#addGameError')).toContainText('legal')

  await page.locator('#addGameNameField').fill('Valid name')
  await page.locator('#addGameButton').click()

  await expect(page.locator('#addGameError')).toContainText('empty')

  await page.locator('#addGameDescriptionField').fill('Test description not too long')
  await page.locator('#addGameButton').click()

  await expect(page.getByText('Valid Name')).toBeVisible()
})

test('remove new game', async ({ page }) => {
  await page.goto('/register')

  await page.locator('#inputUsername').fill('alex')
  await page.locator('#inputEmail').fill('email@example.com')
  await page.locator('#inputPassword').fill('alex.2005')
  await page.locator('#register').click()

  await page.waitForSelector('#login')

  await page.locator('#inputUsername').fill('alex')
  await page.locator('#inputPassword').fill('alex.2005')
  await page.locator('#login').click()

  await page.waitForSelector('#openAddGameModal')

  await page.locator('#openAddGameModal').click()

  await page.locator('#addGameNameField').fill('')
  await page.locator('#addGameDescriptionField').fill('')
  await page.locator('#addGameTagField').fill('Incremental')
  await page.locator('#addGameImageField').fill('../../assets/logo.png')

  await page.locator('#addGameButton').click()

  await expect(page.locator('#addGameError')).toContainText('empty')

  //await page.locator('#addGameNameField').fill('######')
  //await page.locator('#addGameButton').click()

  //await expect(page.locator('#addGameError')).toContainText('legal')

  await page.locator('#addGameNameField').fill('Valid name')
  await page.locator('#addGameButton').click()

  await expect(page.locator('#addGameError')).toContainText('empty')

  await page.locator('#addGameDescriptionField').fill('Test description not too long')
  await page.locator('#addGameButton').click()

  await expect(page.getByText('Valid Name')).toBeVisible()

  await page
    .locator('[data-testid="gameItem"]', {
      hasText: 'Valid Name',
    })
    .locator('[data-testid="removeGameButton"]')
    .click()

  await expect(page.getByText('Valid Name')).toBeHidden() // doesn't exist anymore
})

test('register validation', async ({ page }) => {
  await page.goto('/register')

  await page.locator('#inputUsername').fill('alex')
  await page.locator('#inputEmail').fill('emailexample.com')
  await page.locator('#inputPassword').fill('alex.2005')

  await page.locator('#register').click()
  await expect(page.locator('#errorText')).toContainText('@')

  await page.locator('#inputEmail').fill('correct@working.com')
  await page.locator('#inputPassword').fill('alex')
  await page.locator('#register').click()
  await expect(page.locator('#errorText')).toContainText('8')

  await page.locator('#inputPassword').fill('alexaaaaa')
  await page.locator('#register').click()
  await expect(page.locator('#errorText')).toContainText('@')

  await page.locator('#inputPassword').fill('alexaaaaa')
  await page.locator('#register').click()
  await expect(page.locator('#errorText')).toContainText('@')
})
