import api from './api';

// 1. Pobieranie wszystkich wniosków
export const getAllLeaves = async () => {
    try {
        const response = await api.get('/Leaves');
        return response.data;
    } catch (error) {
        console.error("Błąd pobierania wszystkich wniosków:", error);
        return [];
    }
};

// 2. Pobieranie wniosków konkretnego użytkownika
export const getLeavesByUserId = async (userId) => {
    try {
        const response = await api.get(`/Leaves/user/${userId}`);
        return response.data;
    } catch (error) {
        console.error(`Błąd wniosków użytkownika ${userId}:`, error);
        return [];
    }
};

// 3. Składanie nowego wniosku
export const createLeaveRequest = async (leaveData) => {
    const response = await api.post('/Leaves', leaveData);
    return response.data;
};

// 4. Aktualizacja statusu (PUT)
export const updateLeaveStatus = async (id, status) => {
    const response = await api.put(`/Leaves/${id}/status?status=${status}`);
    return response.data;
};

// 5. TEGO BRAKOWAŁO: Pobieranie wniosków dla konkretnego managera
export const getLeavesByManagerId = async (managerId) => {
    try {
        const response = await api.get('/Leaves');
        const allLeaves = response.data;
        // Filtrujemy wnioski, sprawdzając managerId w obiekcie user
        return allLeaves.filter(leaf => {
            const userObj = leaf.user || leaf.User;
            return userObj && (userObj.managerId === managerId || userObj.ManagerId === managerId);
        });
    } catch (error) {
        console.error("Błąd pobierania wniosków managera:", error);
        return [];
    }
};