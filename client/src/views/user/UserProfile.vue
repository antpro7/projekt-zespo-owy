<template>
  <div v-if="employee" class="d-flex justify-content-center align-items-start pt-5 h-100">
    <div class="card profile-card mb-5 shadow border-0">
      <div class="card-body text-center">
        <div class="mb-4">
            <div class="avatar-placeholder bg-primary text-white rounded-circle d-flex align-items-center justify-content-center mx-auto mb-3">
                <span class="fs-2">{{ employee.firstName ? employee.firstName.charAt(0) : 'U' }}</span>
            </div>
            <h3 class="card-title mb-1">{{ employee.firstName }} {{ employee.lastName }}</h3>
            <p class="text-muted mb-0">{{ employee.position || 'Brak stanowiska' }}</p>
        </div>
        
        <div class="text-start bg-white p-4 rounded-3 shadow-sm mb-4">
            <div class="row mb-2 border-bottom pb-2">
                <div class="col-5 text-muted small uppercase">{{ $t('profile.email') }}</div>
                <div class="col-7 fw-medium">{{ employee.email }}</div>
            </div>
            <div class="row mb-2 border-bottom pb-2 pt-1">
                <div class="col-5 text-muted small">{{ $t('profile.role') }}</div>
                <div class="col-7 fw-medium text-capitalize">{{ $t(`roles.${employee.role}`) }}</div>
            </div>
            <div class="row mb-2 border-bottom pb-2 pt-1">
                <div class="col-5 text-muted small">{{ $t('profile.contract') }}</div>
                <div class="col-7 fw-medium">{{ employee.contractType || '---' }}</div>
            </div>
            <div class="row mb-2 border-bottom pb-2 pt-1">
                <div class="col-5 text-muted small">Godziny miesięczne</div>
                <div class="col-7 fw-medium">{{ employee.monthlyHours }}h</div>
            </div>
            <div class="row pt-1" v-if="employee.role !== 'admin'">
                <div class="col-5 text-muted small">{{ $t('profile.leave_balance') }}</div>
                <div class="col-7 fw-bold" :class="getBalanceColorClass(calculatedLeaveBalance)">
                  {{ calculatedLeaveBalance }} {{ $t('common.days') }}
                </div>
            </div>
        </div>

        <div class="text-start bg-white p-4 rounded-3 shadow-sm">
            <div v-if="!showPasswordForm">
                <button class="btn btn-outline-primary w-100" @click="showPasswordForm = true">
                  {{ $t('profile.change_password') }}
                </button>
            </div>
            <div v-else>
                <div class="d-flex justify-content-between align-items-center mb-3">
                    <h5 class="mb-0">{{ $t('profile.password_modal.title') }}</h5>
                    <button type="button" class="btn-close" @click="showPasswordForm = false"></button>
                </div>
                <form @submit.prevent="handleChangePassword">
                    <div class="mb-3">
                        <label class="form-label small text-muted">{{ $t('profile.password_modal.new_password') }}</label>
                        <input type="password" class="form-control" v-model="newPassword" required minlength="6">
                    </div>
                    <div class="mb-3">
                        <label class="form-label small text-muted">{{ $t('profile.password_modal.confirm_password') }}</label>
                        <input type="password" class="form-control" v-model="confirmPassword" required minlength="6">
                    </div>
                    <div v-if="message" :class="['alert py-2 small', isError ? 'alert-danger' : 'alert-success']">{{ message }}</div>
                    <div class="d-grid gap-2">
                        <button type="submit" class="btn btn-primary">{{ $t('profile.password_modal.submit') }}</button>
                        <button type="button" class="btn btn-light text-muted" @click="showPasswordForm = false">
                          {{ $t('profile.password_modal.cancel') }}
                        </button>
                    </div>
                </form>
            </div>
        </div>

      </div>
    </div>
  </div>
  <div v-else class="text-center pt-5">
    <div class="spinner-border text-primary" role="status"></div>
    <p class="mt-2">{{ $t('common.loading') }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { getCurrentUser, updatePassword } from '../../services/authService';
import { getEmployeeById } from '../../services/employeeService';
import { getLeavesByUserId } from '../../services/leaveService';

const employee = ref(null);
const newPassword = ref('');
const confirmPassword = ref('');
const message = ref('');
const isError = ref(false);
const currentUser = ref(null);
const showPasswordForm = ref(false);
const calculatedLeaveBalance = ref(0);

// Funkcja obliczająca dni (pomocnicza)
const calculateDays = (start, end) => {
  const s = new Date(start);
  const e = new Date(end);
  const diff = Math.abs(e - s);
  return Math.ceil(diff / (1000 * 60 * 60 * 24)) + 1;
};

onMounted(async () => {
  try {
    const user = getCurrentUser();
    if (user) {
      currentUser.value = user;
      // Pobieramy dane usera z bazy (przez Twoją nową metodę GetUser(id) w C#)
      const data = await getEmployeeById(user.id);
      employee.value = data;
      
      if (employee.value) {
          // Pobieramy wnioski, aby obliczyć pozostały urlop
          const leaves = await getLeavesByUserId(user.id);
          const approvedDays = leaves
              .filter(l => l.status === 'Approved' && l.leaveType !== 'Sick')
              .reduce((sum, l) => sum + (l.days || calculateDays(l.startDate, l.endDate)), 0);
          
          // Używamy balansu z bazy (domyślnie 26)
          const initialBalance = employee.value.leaveBalance || 26; 
          calculatedLeaveBalance.value = initialBalance - approvedDays;
      }
    }
  } catch (err) {
    console.error("Błąd ładowania danych profilu:", err);
  }
});

const getBalanceColorClass = (days) => {
    if (days > 10) return 'text-success';
    if (days > 5) return 'text-warning';
    return 'text-danger';
};

const handleChangePassword = async () => {
    message.value = '';
    isError.value = false;

    if (newPassword.value !== confirmPassword.value) {
        message.value = 'Hasła nie są identyczne.';
        isError.value = true;
        return;
    }

    try {
        await updatePassword(currentUser.value.id, newPassword.value);
        message.value = 'Hasło zostało zmienione pomyślnie.';
        newPassword.value = '';
        confirmPassword.value = '';
        setTimeout(() => { showPasswordForm.value = false; message.value = ''; }, 2000);
    } catch (e) {
        message.value = 'Wystąpił błąd podczas zmiany hasła.';
        isError.value = true;
    }
};
</script>

<style scoped>
.profile-card {
    width: 100%;
    max-width: 500px;
    background-color: #f8f9fa;
    border-radius: 15px;
}

.avatar-placeholder {
    width: 100px;
    height: 100px;
    font-size: 2.5rem;
    box-shadow: 0 4px 10px rgba(0,0,0,0.1);
}

.fs-2 {
  font-weight: bold;
}
</style>