<template>
  <div class="container-fluid">
    <h2 class="mb-4">{{ $t('admin.leaves.title') }}</h2>
    
    <div class="card shadow-sm">
      <div class="table-responsive">
        <table class="table table-striped table-hover align-middle mb-0">
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
              <td class="fw-bold">
                {{ request.user ? `${request.user.firstName} ${request.user.lastName}` : (request.userName || '---') }}
              </td>
              <td>
                <span class="badge bg-light text-dark border">
                  {{ request.leaveType ? $t(`leave_requests.types.${request.leaveType}`) : $t('leave_requests.types.undefined') }}
                </span>
              </td>
              <td>
                <small>{{ formatDate(request.startDate) }}</small> 
                <span class="mx-1 text-muted">{{ $t('common.to') }}</span> 
                <small>{{ formatDate(request.endDate) }}</small>
              </td>
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
import { getAllLeaves } from '../../services/leaveService';

const requests = ref([]);

const formatDate = (d) => d ? d.split('T')[0] : '';

const calculateDays = (s, e) => {
  if (!s || !e) return 0;
  return Math.ceil(Math.abs(new Date(e) - new Date(s)) / (1000 * 60 * 60 * 24)) + 1;
};

const loadRequests = async () => {
  try {
    const data = await getAllLeaves();
    requests.value = data;
    console.log("Admin załadował wszystkie wnioski:", data);
  } catch (error) {
    console.error("Błąd admina przy pobieraniu wniosków:", error);
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