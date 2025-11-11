import { useState, useEffect } from "react";

function App() {
  const [names, setNames] = useState([]);
  const [newName, setNewName] = useState("");

const API_URL = `${process.env.REACT_APP_API_URL}/api/names`;

  // Ophalen bij laden
  useEffect(() => {
    fetch(API_URL)
      .then((res) => res.json())
      .then((data) => setNames(data))
      .catch((err) => console.error("Error fetching names:", err));
  }, []);

  // Nieuw naam posten
  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!newName) return;

    const response = await fetch(API_URL, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ value: newName }),
    });

    if (response.ok) {
      const added = await response.json();
      setNames([...names, added]);
      setNewName("");
    } else {
      console.error("Failed to add name");
    }
  };

  return (
    <div style={{ maxWidth: "600px", margin: "2rem auto", fontFamily: "sans-serif" }}>
      <h1>Student QnA - Namenlijst</h1>
      <h2>Hello world!</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Voer een naam in..."
          value={newName}
          onChange={(e) => setNewName(e.target.value)}
          style={{ padding: "0.5rem", width: "70%" }}
        />
        <button type="submit" style={{ padding: "0.5rem", marginLeft: "0.5rem" }}>
          Toevoegen
        </button>
      </form>

      <ul style={{ marginTop: "2rem" }}>
        {names.map((n) => (
          <li key={n.id}>
            {n.value} <small>({new Date(n.createdAt).toLocaleString()})</small>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
