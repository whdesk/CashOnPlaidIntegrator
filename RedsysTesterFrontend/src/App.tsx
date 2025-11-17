import React from 'react'
import RedsysAddCardWithVerify from './components/RedsysAddCardWithVerify'
import RedsysInSiteUnified from './components/RedsysInSiteUnified'

export default function App() {
  return (
    <div style={{ maxWidth: 720, margin: '40px auto', fontFamily: 'Inter, system-ui, Arial' }}>
      <h2>Redsys Tester Frontend</h2>
      <p>Tokenizar tarjeta y realizar cobro con token (MIT).</p>
      <RedsysAddCardWithVerify />
      <RedsysInSiteUnified />
    </div>
  )
}
