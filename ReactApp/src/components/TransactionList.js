import React, { useEffect, useState } from 'react'
import { getTransactions, deleteTransaction } from '../api'
import { toast } from 'react-toastify'

function TransactionList() {
  const [transactions, setTransactions] = useState([])

  useEffect(() => {
    fetchTransactions()
  }, [])

  const fetchTransactions = async () => {
    try {
      const transactions = await getTransactions()
      setTransactions(transactions)
    } catch (error) {
      toast.error('Ошибка при загрузке транзакций')
    }
  }

  const handleDeleteTransaction = async (id) => {
    try {
      await deleteTransaction(id)
      fetchTransactions()
      toast.success('Транзакция успешно удалена')
    } catch (error) {
      toast.error('Ошибка при удалении транзакции')
    }
  }

  return (
    <div className="container mt-4">
      <h2>Список всех транзакций</h2>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Id транзакции</th>
            <th>Дата транзакции</th>
            <th>Сумма</th>
            <th>Действия</th>
          </tr>
        </thead>
        <tbody>
          {transactions.map((transaction) => (
            <tr key={transaction.id}>
              <td>{transaction.id}</td>
              <td>{transaction.transactionDate}</td>
              <td>{transaction.amount}</td>
              <td>
                <button
                  className="btn btn-danger"
                  onClick={() => handleDeleteTransaction(transaction.id)}
                >
                  Удалить
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default TransactionList
