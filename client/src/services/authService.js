import { reactive } from 'vue';
import api from './api'; 

const state = reactive({
    user: JSON.parse(localStorage.getItem('user')) || null
});

export const login = async (email, password) => {
    try {
        const response = await api.post('/Auth/login', { email, password });
        const user = response.data;
        if (user) {
            state.user = user;
            localStorage.setItem('user', JSON.stringify(user));
            return user;
        }
        return null;
    } catch (error) {
        console.error("Błąd logowania:", error.response?.data || error.message);
        return null;
    }
};

export const logout = () => {
    state.user = null;
    localStorage.removeItem('user');
};

export const getCurrentUser = () => state.user;

// DODAJ TĘ FUNKCJĘ TUTAJ:
export const resetPassword = async (userId) => {
    try {
        // Ważne: Endpoint musi być zgodny z Twoim kontrolerem w C# (np. /Users/5/reset-password)
        const response = await api.post(`/Users/${userId}/reset-password`);
        return response.data;
    } catch (error) {
        console.error("Błąd resetowania hasła:", error.response?.data || error.message);
        throw error;
    }
};

export const updatePassword = async (id, newPassword) => {
    try {
        const response = await api.put(`/Users/${id}/change-password`, { newPassword });
        return response.data;
    } catch (error) {
        console.error("Błąd zmiany hasła:", error);
        return false;
    }
};