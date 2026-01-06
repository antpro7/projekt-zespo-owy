<template>
  <div class="container-fluid">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h2>{{ $t('admin.employees.title') }}</h2>
      <button class="btn btn-primary shadow-sm" @click="openModal()">
        <i class="bi bi-person-plus-fill"></i> {{ $t('admin.employees.add_btn') }}
      </button>
    </div>

    <div class="card shadow-sm border-0">
      <div class="table-responsive">
        <table class="table table-hover align-middle mb-0">
          <thead class="table-light">
            <tr>
              <th>ID</th>
              <th>{{ $t('admin.employees.table.name') }}</th>
              <th>{{ $t('admin.employees.table.email') }}</th>
              <th>{{ $t('admin.employees.table.role') }}</th>
              <th>{{ $t('admin.employees.table.position') }}</th>
              <th class="text-end">{{ $t('admin.employees.table.actions') }}</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="employee in employees" :key="employee.id">
              <td>{{ employee.id }}</td>
              <td class="fw-bold">
                {{ employee.firstName }} {{ employee.lastName }}
              </td>
              <td>{{ employee.email }}</td>
              <td>
                <span :class="getRoleBadgeClass(employee.role)">
                  {{ $t(`roles.${employee.role}`) }}
                </span>
              </td>
              <td>{{ employee.position }}</td>
              <td class="text-end">
                <button class="btn btn-sm btn-outline-primary me-2" @click="openModal(employee)" :title="$t('common.edit')">
                  <i class="bi bi-pencil"></i>
                </button>
                <button class="btn btn-sm btn-outline-dark" @click="handleResetPassword(employee.id, employee.firstName)" :title="$t('admin.employees.reset_password')">
                  <i class="bi bi-key"></i>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div class="modal fade" id="employeeModal" tabindex="-1" aria-hidden="true" ref="modalRef">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header bg-light">
            <h5 class="modal-title">
              {{ isEditing ? $t('admin.employees.modal.edit_title') : $t('admin.employees.modal.add_title') }}
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <form @submit.prevent="saveEmployee" id="employeeForm" class="row g-3">
              <div class="col-md-6">
                <label class="form-label">Imię</label>
                <input type="text" class="form-control" v-model="form.firstName" required>
              </div>
              <div class="col-md-6">
                <label class="form-label">Nazwisko</label>
                <input type="text" class="form-control" v-model="form.lastName" required>
              </div>
              <div class="col-md-12">
                <label class="form-label">{{ $t('admin.employees.table.email') }}</label>
                <input type="email" class="form-control" v-model="form.email" required>
              </div>
              <div class="col-md-6">
                <label class="form-label">{{ $t('admin.employees.table.role') }}</label>
                <select class="form-select" v-model="form.role" required>
                  <option value="employee">{{ $t('roles.employee') }}</option>
                  <option value="manager">{{ $t('roles.manager') }}</option>
                  <option value="admin">{{ $t('roles.admin') }}</option>
                </select>
              </div>
              
              <div class="col-md-6" v-if="form.role === 'employee'">
                <label class="form-label">{{ $t('admin.employees.modal.manager_label') }}</label>
                <select class="form-select" v-model="form.managerId">
                  <option :value="null">{{ $t('admin.employees.modal.select_manager') }}</option>
                  <option v-for="manager in managers" :key="manager.id" :value="manager.id">
                    {{ manager.firstName }} {{ manager.lastName }}
                  </option>
                </select>
              </div>

              <div class="col-md-6">
                <label class="form-label">{{ $t('admin.employees.table.position') }}</label>
                <input type="text" class="form-control" v-model="form.position" required>
              </div>
              <div class="col-md-6">
                <label class="form-label">Typ umowy</label>
                <select class="form-select" v-model="form.contractType" required>
                  <option value="UoP">UoP</option>
                  <option value="B2B">B2B</option>
                  <option value="Mandate">Mandate</option>
                </select>
              </div>
              <div class="col-md-6">
                <label class="form-label">Wymiar godzin</label>
                <input type="number" class="form-control" v-model="form.hours" required>
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">{{ $t('common.cancel') }}</button>
            <button type="submit" form="employeeForm" class="btn btn-primary">{{ $t('common.save') }}</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, reactive } from 'vue';
import { getEmployees, createEmployee, updateEmployee, getManagers } from '../../services/employeeService';
import { resetPassword } from '../../services/authService';
import { Modal } from 'bootstrap';

// Stan komponentu
const employees = ref([]);
const managers = ref([]);
const modalRef = ref(null);
let modalInstance = null;
const isEditing = ref(false);

// Formularz reaktywny
const form = reactive({
  id: null,
  firstName: '',
  lastName: '',
  email: '',
  role: 'employee',
  position: '',
  contractType: 'UoP',
  hours: 160,
  managerId: null
});

// Ładowanie danych z API
const loadData = async () => {
  try {
    const [empData, mgrData] = await Promise.all([getEmployees(), getManagers()]);
    employees.value = empData || [];
    managers.value = mgrData || [];
  } catch (error) {
    console.error("Błąd podczas ładowania danych:", error);
  }
};

// Obsługa modala
const openModal = (employee = null) => {
  if (employee) {
    isEditing.value = true;
    // Kopiujemy dane pracownika do formularza
    Object.assign(form, {
      id: employee.id,
      firstName: employee.firstName,
      lastName: employee.lastName,
      email: employee.email,
      role: employee.role,
      position: employee.position,
      contractType: employee.contractType || 'UoP',
      hours: employee.hours || 160,
      managerId: employee.managerId
    });
  } else {
    isEditing.value = false;
    Object.assign(form, {
      id: null, firstName: '', lastName: '', email: '',
      role: 'employee', position: '', contractType: 'UoP',
      hours: 160, managerId: null
    });
  }
  modalInstance.show();
};

// Zapisywanie (Create/Update)
const saveEmployee = async () => {
  try {
    if (isEditing.value) {
      await updateEmployee(form.id, form);
    } else {
      await createEmployee(form);
    }
    modalInstance.hide();
    await loadData();
  } catch (error) {
    console.error("Błąd zapisu:", error);
    alert("Wystąpił błąd podczas zapisywania danych pracownika.");
  }
};

// Resetowanie hasła
const handleResetPassword = async (id, name) => {
  if (confirm(`Czy na pewno chcesz zresetować hasło dla ${name}?`)) {
    try {
      await resetPassword(id);
      alert(`Hasło dla użytkownika ${name} zostało zresetowane do: 'password'`);
    } catch (error) {
      console.error("Błąd resetu hasła:", error);
      alert("Nie udało się zresetować hasła.");
    }
  }
};

// Stylistyka badge ról
const getRoleBadgeClass = (role) => {
  switch (role) {
    case 'admin': return 'badge bg-danger';
    case 'manager': return 'badge bg-warning text-dark';
    default: return 'badge bg-success';
  }
};

onMounted(async () => {
  await loadData();
  if (modalRef.value) {
    modalInstance = new Modal(modalRef.value);
  }
});
</script>