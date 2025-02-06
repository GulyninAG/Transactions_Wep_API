import React, { useState } from 'react'
import { getTransaction, createTransaction } from '../api'
import { generate_guid } from '../utils'
import { toast } from 'react-toastify'

function TransactionForm({ onTransactionCreated }) {
  const [transactionDate, setTransactionDate] = useState('')
  const [transactionAmount, setTransactionAmount] = useState('')
  const [oneTransactionId, setOneTransactionId] = useState('')
  const [oneTransaction, setOneTransaction] = useState(null)

  const handleSaveTransaction = async () => {
    if (transactionDate && transactionAmount) {
      const transaction = {
        Id: generate_guid(),
        TransactionDate: transactionDate,
        Amount: parseFloat(transactionAmount),
      }

      try {
        await createTransaction(transaction)
        onTransactionCreated()
        setTransactionDate('')
        setTransactionAmount('')
        toast.success('Транзакция успешно добавлена!')
      } catch (error) {
        toast.error('Ошибка при добавлении транзакции')
      }
    } else {
      toast.warning('Заполните все поля')
    }
  }

  const handleGetTransaction = async () => {
    if (!oneTransactionId) {
      toast.warning('Введите ID транзакции')
      return
    }

    try {
      const transaction = await getTransaction(oneTransactionId)
      setOneTransaction(transaction)
      toast.success('Транзакция найдена')
    } catch (error) {
      toast.error('Транзакция не найдена')
    }
  }

  return (
    <div className="container mt-4">
      <h2>Сохранить транзакцию</h2>
      <div className="mb-3">
        <label htmlFor="transactionDate" className="form-label">
          Дата транзакции
        </label>
        <input
          type="date"
          className="form-control"
          id="transactionDate"
          value={transactionDate}
          onChange={(e) => setTransactionDate(e.target.value)}
          required
        />
      </div>
      <div className="mb-3">
        <label htmlFor="transactionAmount" className="form-label">
          Сумма
        </label>
        <input
          type="number"
          className="form-control"
          id="transactionAmount"
          value={transactionAmount}
          onChange={(e) => setTransactionAmount(e.target.value)}
          required
        />
      </div>
      <button className="btn btn-primary" onClick={handleSaveTransaction}>
        Сохранить транзакцию
      </button>

      <h2 className="mt-4">Получить транзакцию по ID</h2>
      <div className="mb-3">
        <label htmlFor="oneTransactionId" className="form-label">
          Id транзакции
        </label>
        <input
          type="text"
          className="form-control"
          id="oneTransactionId"
          value={oneTransactionId}
          onChange={(e) => setOneTransactionId(e.target.value)}
        />
      </div>
      <button className="btn btn-primary" onClick={handleGetTransaction}>
        Получить транзакцию
      </button>
      {oneTransaction && (
        <div className="mt-3">
          <div className="mb-3">
            <label htmlFor="oneTransactionDate" className="form-label">
              Дата транзакции
            </label>
            <input
              type="text"
              className="form-control"
              id="oneTransactionDate"
              value={oneTransaction.transactionDate}
              readOnly
            />
          </div>
          <div className="mb-3">
            <label htmlFor="oneTransactionAmount" className="form-label">
              Сумма
            </label>
            <input
              type="text"
              className="form-control"
              id="oneTransactionAmount"
              value={oneTransaction.amount}
              readOnly
            />
          </div>
        </div>
      )}
    </div>
  )
}

export default TransactionForm
