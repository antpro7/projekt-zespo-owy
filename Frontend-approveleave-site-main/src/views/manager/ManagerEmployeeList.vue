<template>
  <div>
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h2>{{ $t('manager.employees.title') }}</h2>
    </div>

    <div v-if="employees.length === 0" class="alert alert-info">
      Brak przypisanych pracowników.
    </div>

    <table v-else class="table table-striped table-hover shadow-sm">
      <thead class="table-light">
        <tr>
          <th>{{ $t('admin.employees.table.id') }}</th>
          <th>{{ $t('admin.employees.table.name') }}</th>
          <th>{{ $t('admin.employees.table.email') }}</th>
          <th>{{ $t('admin.employees.table.role') }}</th>
          <th>{{ $t('admin.employees.table.position') }}</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="employee in employees" :key="employee.id">
          <td>{{ employee.id }}</td>
          <td>{{ employee.firstName }} {{ employee.lastName }}</td>
          <td>{{ employee.email }}</td>
          <td>
            <span :class="getRoleBadgeClass(employee.role)">
              {{ $t(`roles.${employee.role}`) }}
            </span>
          </td>
          <td>{{ employee.position || '---' }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { getEmployeesByManagerId } from '../../services/employeeService';
import { getCurrentUser } from '../../services/authService';

const employees = ref([]);
const currentUser = getCurrentUser();

const loadEmployees = async () => {
  try {
    if (currentUser && currentUser.id) {
      // Wywołanie funkcji z employeeService.js
      const data = await getEmployeesByManagerId(currentUser.id);
      employees.value = data;
    }
  } catch (error) {
    console.error("Błąd ładowania pracowników:", error);
  }
};

const getRoleBadgeClass = (role) => {
  switch (role) {
    case 'admin': return 'badge bg-danger text-uppercase';
    case 'manager': return 'badge bg-warning text-dark text-uppercase';
    default: return 'badge bg-success text-uppercase';
  }
};

onMounted(() => {
  loadEmployees();
});
</script>

<style scoped>
.table th {
  font-weight: 600;
  text-transform: uppercase;
  font-size: 0.85rem;
}
.badge {
  padding: 0.5em 0.8em;
}
</style>