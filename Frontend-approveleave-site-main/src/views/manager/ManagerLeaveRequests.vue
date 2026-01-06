<template>
  <div>
    <h2 class="mb-4">{{ $t('manager.team_requests.title') }}</h2>
    <div class="card shadow-sm">
      <div class="table-responsive">
        <table class="table table-striped align-middle mb-0">
          <thead class="table-light">
            <tr>
              <th>{{ $t('admin.employees.table.id') }}</th>
              <th>{{ $t('common.employee') }}</th>
              <th>{{ $t('common.type') }}</th>
              <th>{{ $t('common.dates') }}</th>
              <th>{{ $t('common.days') }}</th>
              <th>{{ $t('common.status') }}</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="request in requests" :key="request.id">
              <td>{{ request.id }}</td>
              <td>
                {{ request.user ? `${request.user.firstName} ${request.user.lastName}` : (request.userName || '---') }}
              </td>
              <td>
                <span v-if="request.leaveType">
                  {{ $t(`leave_requests.types.${request.leaveType}`) }}
                </span>
                <span v-else class="text-muted">---</span>
              </td>
              <td>{{ formatDate(request.startDate) }} {{ $t('common.to') }} {{ formatDate(request.endDate) }}</td>
              <td>{{ request.days || calculateDays(request.startDate, request.endDate) }}</td>
              <td>
                <span :class="getStatusBadgeClass(request.status)">
                  {{ $t(`status.${request.status}`) }}
                </span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { getLeavesByManagerId } from '../../services/leaveService';
import { getCurrentUser } from '../../services/authService';

const requests = ref([]);
const currentUser = getCurrentUser();

// Pomocnicze funkcje, żeby daty i dni wyglądały ładnie
const formatDate = (d) => d ? d.split('T')[0] : '';
const calculateDays = (s, e) => {
  if (!s || !e) return 0;
  return Math.ceil(Math.abs(new Date(e) - new Date(s)) / (1000 * 60 * 60 * 24)) + 1;
};

const loadRequests = async () => {
  if (currentUser && (currentUser.role === 'manager' || currentUser.role === 'admin')) {
    // API zwraca obiekty z polem leaveType
    const data = await getLeavesByManagerId(currentUser.id);
    requests.value = data;
    console.log("Załadowane wnioski zespołu:", data);
  }
};

const getStatusBadgeClass = (status) => {
  switch (status) {
    case 'Approved': return 'badge bg-success';
    case 'Rejected': return 'badge bg-danger';
    case 'Pending': return 'badge bg-warning text-dark';
    default: return 'badge bg-secondary';
  }
};

onMounted(() => {
  loadRequests();
});
</script>