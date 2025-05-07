// src/stores/VehicleOwnerStore.js
import { makeAutoObservable, runInAction } from 'mobx';
import api from '../common/services/api';

class VehicleOwnerStore {
  owners = [];
  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  makeFilter = '';
  modelFilter = '';
  firstNameFilter = '';
  lastNameFilter = '';
  sortBy = '';
  sortDescending = false;
  loading = false;
  error = null;

  currentOwner = { id: 0, firstName: '', lastName: '', dob: '' };

  constructor() {
    makeAutoObservable(this);
  }

  get totalPages() {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  get sortedOwners() {
    const list = [...this.owners];
    if (!this.sortBy) return list;

    function getFieldVal(o, field) {
      if (field === 'makeList') {
        return (o.cars.length ? o.cars.map(c => c.make).join(', ') : '').toLowerCase();
      }
      if (field === 'modelList') {
        return (o.cars.length ? o.cars.map(c => c.model).join(', ') : '').toLowerCase();
      }
      return String(o[field] || '').toLowerCase();
    }

    list.sort((a, b) => {
      const aVal = getFieldVal(a, this.sortBy);
      const bVal = getFieldVal(b, this.sortBy);
      if (aVal < bVal) return this.sortDescending ? 1 : -1;
      if (aVal > bVal) return this.sortDescending ? -1 : 1;
      return 0;
    });
    return list;
  }

  setClientSort(field) {
    if (this.sortBy === field) {
      this.sortDescending = !this.sortDescending;
    } else {
      this.sortBy = field;
      this.sortDescending = false;
    }
  }

  async fetchOwners() {
    this.loading = true;
    this.error = null;
    try {
      const params = {
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
      };
      if (this.makeFilter) params.makeId = this.makeFilter;
      if (this.modelFilter) params.modelId = this.modelFilter;
      if (this.firstNameFilter) params.firstName = this.firstNameFilter;
      if (this.lastNameFilter) params.lastName = this.lastNameFilter;

      const { data } = await api.get('/VehicleOwners', { params });
      runInAction(() => {
        this.owners = data.items;
        this.totalCount = data.totalCount;
      });
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  }

  setPage(page) {
    this.pageNumber = page;
    this.fetchOwners();
  }

  setPageSize(size) {
    this.pageSize = size;
    this.pageNumber = 1;
    this.fetchOwners();
  }

  setMakeFilter(id) {
    this.makeFilter = id;
    this.pageNumber = 1;
    this.fetchOwners();
  }

  setModelFilter(id) {
    this.modelFilter = id;
    this.pageNumber = 1;
    this.fetchOwners();
  }

  setFirstNameFilter(text) {
    this.firstNameFilter = text;
    this.pageNumber = 1;
    this.fetchOwners();
  }

  setLastNameFilter(text) {
    this.lastNameFilter = text;
    this.pageNumber = 1;
    this.fetchOwners();
  }

  // **only** flip in‑memory sort, no API call
  setSort(field) {
    if (this.sortBy === field) {
      this.sortDescending = !this.sortDescending;
    } else {
      this.sortBy = field;
      this.sortDescending = false;
    }
  }

  async createOwner(dto) {
    const { data } = await api.post('/VehicleOwners', dto);
    runInAction(() => {
      this.pageNumber = 1;
    });
    this.fetchOwners();
    return data;
  }

  async updateOwnerById(id, dto) {
    await api.put(`/VehicleOwners/${id}`, dto);
    this.fetchOwners();
  }

  async deleteOwner(id) {
    await api.delete(`/VehicleOwners/${id}`);
    const newTotal = this.totalCount - 1;
    const newPages = Math.max(Math.ceil(newTotal / this.pageSize), 1);
    if (this.pageNumber > newPages) this.pageNumber = newPages;
    this.fetchOwners();
  }

  async loadOwnerForEdit(id) {
    this.loading = true;
    try {
      const { data } = await api.get(`/VehicleOwners/${id}`);
      runInAction(() => {
        this.currentOwner = {
          id: data.id,
          firstName: data.firstName,
          lastName: data.lastName,
          dob: data.dob.slice(0, 10),
        };
      });
    } catch {}
    runInAction(() => {
      this.loading = false;
    });
  }

  setEditFirstName(v) {
    this.currentOwner.firstName = v;
  }
  setEditLastName(v) {
    this.currentOwner.lastName = v;
  }
  setEditDob(v) {
    this.currentOwner.dob = v;
  }

  async saveEdit() {
    const dto = {
      firstName: this.currentOwner.firstName,
      lastName: this.currentOwner.lastName,
      dob: new Date(this.currentOwner.dob),
    };
    await api.put(`/VehicleOwners/${this.currentOwner.id}`, dto);
    this.fetchOwners();
  }
}

export default new VehicleOwnerStore();
