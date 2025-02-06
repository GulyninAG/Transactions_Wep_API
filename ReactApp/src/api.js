// Для работы с API
const apiUrl = 'http://localhost:23188'

export async function getTransactions() {
  const response = await fetch(`${apiUrl}/api/v1/Transactions`, {
    method: 'GET',
    headers: { Accept: 'application/json' },
  })

  if (response.ok) {
    return await response.json()
  } else {
    throw new Error('Failed to fetch transactions')
  }
}

export async function getTransaction(id) {
  const response = await fetch(`${apiUrl}/api/v1/Transaction?id=${id}`, {
    method: 'GET',
    headers: { Accept: 'application/json' },
  })

  if (response.ok) {
    return await response.json()
  } else {
    throw new Error('Failed to fetch transaction')
  }
}

export async function createTransaction(transaction) {
  const response = await fetch(`${apiUrl}/api/v1/Transaction`, {
    method: 'POST',
    headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
    body: JSON.stringify(transaction),
  })

  if (response.ok) {
    return await response.json()
  } else {
    throw new Error('Failed to create transaction')
  }
}

export async function deleteTransaction(id) {
  const response = await fetch(`${apiUrl}/api/v1/Transaction/Delete/${id}`, {
    method: 'DELETE',
    headers: { Accept: 'application/json' },
  })

  if (response.ok) {
    return await response.json()
  } else {
    throw new Error('Failed to delete transaction')
  }
}
