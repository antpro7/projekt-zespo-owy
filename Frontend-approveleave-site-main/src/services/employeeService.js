import api from './api';

// Pobierz wszystkich pracowników
export const getEmployees = async () => {
    try {
        const response = await api.get('/Users'); // Upewnij się, że endpoint to /Users lub /Employees
        return response.data;
    } catch (error) {
        console.error("Błąd pobierania pracowników:", error);
        return [];
    }
};

// Pobierz tylko managerów (potrzebne do listy rozwijanej w modalu)
export const getManagers = async () => {
    try {
        const response = await api.get('/Users');
        // Filtrujemy użytkowników, którzy mają rolę manager
        return response.data.filter(u => u.role === 'manager');
    } catch (error) {
        console.error("Błąd pobierania managerów:", error);
        return [];
    }
};

// DODAWANIE nowego pracownika
export const createEmployee = async (employeeData) => {
    try {
        const response = await api.post('/Users', employeeData);
        return response.data;
    } catch (error) {
        console.error("Błąd podczas tworzenia pracownika:", error);
        throw error;
    }
};

// AKTUALIZACJA pracownika (Tego brakowało!)
export const updateEmployee = async (id, employeeData) => {
    try {
        const response = await api.put(`/Users/${id}`, employeeData);
        return response.data;
    } catch (error) {
        console.error(`Błąd podczas aktualizacji pracownika o ID ${id}:`, error);
        throw error;
    }
};

// Reset hasła
export const resetPassword = async (userId) => {
    try {
        const response = await api.post(`/Users/${userId}/reset-password`);
        return response.data;
    } catch (error) {
        console.error("Błąd resetowania hasła:", error);
        throw error;
    }
};