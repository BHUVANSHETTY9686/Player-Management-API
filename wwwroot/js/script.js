let token = '';
const API_URL = window.location.origin;

// UI Elements
const loginForm = document.getElementById('loginForm');
const playersSection = document.getElementById('playersSection');
const loginBtn = document.getElementById('loginBtn');
const logoutBtn = document.getElementById('logoutBtn');
const playersTableBody = document.getElementById('playersTableBody');
const addPlayerModal = new bootstrap.Modal(document.getElementById('addPlayerModal'));

// Event Listeners
loginBtn.addEventListener('click', () => {
    loginForm.classList.remove('d-none');
    playersSection.classList.add('d-none');
});

logoutBtn.addEventListener('click', () => {
    token = '';
    loginForm.classList.remove('d-none');
    playersSection.classList.add('d-none');
    loginBtn.classList.remove('d-none');
    logoutBtn.classList.add('d-none');
});

// Authentication
async function login() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    try {
        const response = await fetch(`${API_URL}/api/auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        });

        if (response.ok) {
            const data = await response.json();
            token = data.token;
            loginForm.classList.add('d-none');
            playersSection.classList.remove('d-none');
            loginBtn.classList.add('d-none');
            logoutBtn.classList.remove('d-none');
            loadPlayers();
        } else {
            showAlert('Login failed. Please check your credentials.', 'danger');
        }
    } catch (error) {
        showAlert('An error occurred during login.', 'danger');
    }
}

// Players CRUD Operations
async function loadPlayers() {
    try {
        const response = await fetch(`${API_URL}/api/players`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            const players = await response.json();
            displayPlayers(players);
        } else {
            showAlert('Failed to load players.', 'danger');
        }
    } catch (error) {
        showAlert('An error occurred while loading players.', 'danger');
    }
}

function displayPlayers(players) {
    playersTableBody.innerHTML = '';
    players.forEach(player => {
        playersTableBody.innerHTML += `
            <tr>
                <td>${player.name}</td>
                <td>${player.position}</td>
                <td>${player.team}</td>
                <td>${player.jerseyNumber}</td>
                <td>${player.age}</td>
                <td>
                    <button class="btn btn-sm btn-danger" onclick="deletePlayer(${player.id})">Delete</button>
                </td>
            </tr>
        `;
    });
}

async function addPlayer() {
    const player = {
        name: document.getElementById('playerName').value,
        position: document.getElementById('playerPosition').value,
        team: document.getElementById('playerTeam').value,
        jerseyNumber: parseInt(document.getElementById('playerJerseyNumber').value),
        age: parseInt(document.getElementById('playerAge').value),
        joinDate: new Date().toISOString()
    };

    try {
        const response = await fetch(`${API_URL}/api/players`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(player)
        });

        if (response.ok) {
            addPlayerModal.hide();
            document.getElementById('addPlayerForm').reset();
            loadPlayers();
            showAlert('Player added successfully!', 'success');
        } else {
            showAlert('Failed to add player.', 'danger');
        }
    } catch (error) {
        showAlert('An error occurred while adding the player.', 'danger');
    }
}

async function deletePlayer(id) {
    if (confirm('Are you sure you want to delete this player?')) {
        try {
            const response = await fetch(`${API_URL}/api/players/${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (response.ok) {
                loadPlayers();
                showAlert('Player deleted successfully!', 'success');
            } else {
                showAlert('Failed to delete player.', 'danger');
            }
        } catch (error) {
            showAlert('An error occurred while deleting the player.', 'danger');
        }
    }
}

// Utility Functions
function showAlert(message, type) {
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type} alert-dismissible fade show`;
    alertDiv.role = 'alert';
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    document.body.appendChild(alertDiv);

    setTimeout(() => {
        alertDiv.remove();
    }, 3000);
}
