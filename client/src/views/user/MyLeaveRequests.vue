<template>
  <div>
    <h2>{{ $t('leave_requests.title') }}</h2>
    
    <div class="card mb-4 mt-3">
      <div class="card-header">{{ $t('leave_requests.request_leave') }}</div>
      <div class="card-body">
        <form @submit.prevent="submitRequest">
          <div class="row">
            <div class="col-md-4 mb-3">
              <label class="form-label">{{ $t('leave_requests.type_label') }}</label>
              <select class="form-select" v-model="form.leaveType" required>
                <option value="Annual">{{ $t('leave_requests.types.Annual') }}</option>
                <option value="Sick">{{ $t('leave_requests.types.Sick') }}</option>
                <option value="Unpaid">{{ $t('leave_requests.types.Unpaid') }}</option>
              </select>
            </div>
            <div class="col-md-4 mb-3">
              <label class="form-label">{{ $t('leave_requests.start_date') }}</label>
              <input type="date" class="form-control" v-model="form.startDate" required>
            </div>
            <div class="col-md-4 mb-3">
              <label class="form-label">{{ $t('leave_requests.end_date') }}</label>
              <input type="date" class="form-control" v-model="form.endDate" required>
            </div>
          </div>
          <button type="submit" class="btn btn-primary">{{ $t('leave_requests.submit') }}</button>
        </form>
      </div>
    </div>

    <h3>{{ $t('leave_requests.history') }}</h3>
    <table class="table table-striped">
      <thead>
        <tr>
          <th>{{ $t('common.type') }}</th>
          <th>{{ $t('common.dates') }}</th>
          <th>{{ $t('common.days') }}</th>
          <th>{{ $t('common.status') }}</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="request in requests" :key="request.id">
          <td>{{ $t(`leave_requests.types.${request.leaveType}`) }}</td>
          <td>{{ formatDate(request.startDate) }} {{ $t('common.to') }} {{ formatDate(request.endDate) }}</td>
          <td>{{ request.days || calculateDays(request.startDate, request.endDate) }}</td>
          <td>
            <span :class="getStatusBadgeClass(request.status)">{{ $t(`status.${request.status}`) }}</span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue';
import { getCurrentUser } from '../../services/authService';
import { getLeavesByUserId, createLeaveRequest } from '../../services/leaveService';

const requests = ref([]);
const form = reactive({
  leaveType: 'Annual', // Zmienione z type
  startDate: '',
  endDate: ''
});

// Funkcja pomocnicza do formatowania daty (opcjonalnie)
const formatDate = (dateString) => {
  if (!dateString) return '';
  return dateString.split('T')[0];
};

// Obliczanie dni roboczych/kalendarzowych
const calculateDays = (start, end) => {
  if (!start || !end) return 0;
  const s = new Date(start);
  const e = new Date(end);
  const diffTime = Math.abs(e - s);
  return Math.ceil(diffTime / (1000 * 60 * 60 * 24)) + 1;
};

const loadRequests = async () => {
  const user = getCurrentUser();
  if (user) {
    // Pobieranie z API (C#)
    const data = await getLeavesByUserId(user.id);
    requests.value = data;
  }
};

const submitRequest = async () => {
  const user = getCurrentUser();
  if (!user) return;

  const days = calculateDays(form.startDate, form.endDate);

  const newRequest = {
    userId: user.id,
    leaveType: form.leaveType, // Zgodne z LeaveRequest.cs
    startDate: form.startDate,
    endDate: form.endDate,
    status: 'Pending',
    comment: '' // Możesz dodać pole w formularzu, jeśli chcesz
  };

  try {
    await createLeaveRequest(newRequest);
    
    // Reset formularza
    form.startDate = '';
    form.endDate = '';
    form.leaveType = 'Annual';
    
    // Odświeżenie listy
    await loadRequests();
  } catch (error) {
    alert("Błąd podczas składania wniosku. Sprawdź połączenie z API.");
  }
};

const getStatusBadgeClass = (status) => {
  switch (status) {
    case 'Approved': return 'badge bg-success';
    case 'Rejected': return 'badge bg-danger';
    default: return 'badge bg-warning text-dark';
  }
};

onMounted(() => {
  loadRequests();
});
</script>