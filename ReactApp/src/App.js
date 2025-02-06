import './App.css'
import 'react-toastify/dist/ReactToastify.css'
import React, { useState } from 'react'
import { ToastContainer, toast } from 'react-toastify'
import TransactionForm from './components/TransactionForm'
import TransactionList from './components/TransactionList'

function App() {
  const [refreshList, setRefreshList] = useState(false)

  const handleTransactionCreated = () => {
    setRefreshList(!refreshList)
  }

  return (
    <div className="container">
      <h1>Transactions WebAPI (React Application)</h1>
      <TransactionForm onTransactionCreated={handleTransactionCreated} />
      <TransactionList key={refreshList} />
      <ToastContainer
        position="top-right"
        autoClose={3000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
      />
    </div>
  )
}

export default App
