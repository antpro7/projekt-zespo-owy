<template>
  <div class="container-fluid">
    <h2 class="mb-4">{{ $t('manager.approvals.title') }}</h2>
    
    <div v-if="loading" class="text-center my-5">
      <div class="spinner-border text-primary" role="status"></div>
      <p class="mt-2">Ładowanie wniosków zespołu...</p>
    </div>

    <div v-else>
      <div v-if="pendingRequests.length === 0" class="alert alert-info shadow-sm">
        {{ $t('manager.approvals.no_pending') }}
      </div>

      <div v-else class="card shadow-sm border-0">
        <div class="table-responsive">
          <table class="table table-hover align-middle mb-0">
            <thead class="table-light">
              <tr>
                <th>{{ $t('common.employee') }}</th>
                <th>{{ $t('common.type') }}</th>
                <th>{{ $t('common.dates') }}</th>
                <th>{{ $t('common.days') }}</th>
                <th class="text-end">{{ $t('common.actions') }}</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="request in pendingRequests" :key="request.id">
                <td>
                  <div class="fw-bold">
                    {{ request.user ? `${request.user.firstName} ${request.user.lastName}` : 'Użytkownik' }}
                  </div>
                </td>
                <td>
                  <span class="badge bg-light text-dark border">
                    {{ request.leaveType ? $t(`leave_requests.types.${request.leaveType}`) : $t('leave_requests.types.undefined') }}
                  </span>
                </td>
                <td>
                  <small class="text-muted">
                    {{ formatDate(request.startDate) }} - {{ formatDate(request.endDate) }}
                  </small>
                </td>
                <td>
                  {{ request.days || calculateDays(request.startDate, request.endDate) }}
                </td>
                <td class="text-end">
                  <button 
                    class="btn btn-sm btn-success me-2 px-3" 
                    @click="handleApproval(request.id, 'Approved')"
                  >
                    {{ $t('manager.approvals.approve') }}
                  </button>
                  <button 
                    class="btn btn-sm btn-outline-danger px-3" 
                    @click="handleApproval(request.id, 'Rejected')"
                  >
                    {{ $t('manager.approvals.reject') }}
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { getLeavesByManagerId, updateLeaveStatus } from '../../services/leaveService';
import { getCurrentUser } from '../../services/authService';

const allRequests = ref([]);
const loading = ref(true);
const currentUser = getCurrentUser();

const formatDate = (d) => d ? d.split('T')[0] : '';

const calculateDays = (s, e) => {
  if (!s || !e) return 0;
  const start = new Date(s);
  const end = new Date(e);
  const diff = Math.abs(end - start);
  return Math.ceil(diff / (1000 * 60 * 60 * 24)) + 1;
};

// Filtrujemy wnioski Pending (używamy małej litery 'status')
const pendingRequests = computed(() => {
  return allRequests.value.filter(r => r.status === 'Pending');
});

const loadRequests = async () => {
  loading.value = true;
  try {
    if (currentUser && currentUser.id) {
      // Pobieramy dane przefiltrowane przez managerId
      allRequests.value = await getLeavesByManagerId(currentUser.id);
      console.log("Wnioski załadowane pomyślnie:", allRequests.value);
    }
  } catch (error) {
    console.error("Błąd podczas ładowania wniosków:", error);
  } finally {
    loading.value = false;
  }
};

const handleApproval = async (id, status) => {
  if (!confirm(`Czy na pewno chcesz ustawić status: ${status}?`)) return;
  
  try {
    await updateLeaveStatus(id, status);
    await loadRequests();
  } catch (error) {
    console.error("Błąd aktualizacji statusu:", error);
    alert("Wystąpił błąd podczas zapisywania decyzji.");
  }
};

onMounted(loadRequests);
</script>