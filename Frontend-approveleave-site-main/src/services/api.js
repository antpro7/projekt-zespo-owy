import axios from 'axios';

const api = axios.create({
    // Tu wpisz adres, który pojawia się po uruchomieniu Twojego projektu w Visual Studio
    baseURL: 'https://localhost:7034/api', 
    headers: {
        'Content-Type': 'application/json'
    }
});

export default api;