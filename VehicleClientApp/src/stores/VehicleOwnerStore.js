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

  constructor() {
    makeAutoObservable(this);
  }

  get totalPages() {
    return Math.ceil(this.totalCount / this.pageSize);
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
      if (this.sortBy) {
        params.sortBy = this.sortBy;
        params.sortDescending = this.sortDescending;
      }

      const { data } = await api.get('/VehicleOwners', { params });
      runInAction(() => {
        this.owners = data.items;
        this.totalCount = data.totalCount;
      });
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
      console.error('Error loading owners', err);
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

  setSort(field) {
    if (this.sortBy === field) {
      this.sortDescending = !this.sortDescending;
    } else {
      this.sortBy = field;
      this.sortDescending = false;
    }
    this.fetchOwners();
  }

  async createOwner(dto) {
    const { data } = await api.post('/VehicleOwners', dto);
    runInAction(() => {
      this.pageNumber = 1;
    });
    this.fetchOwners();
    return data;
  }

  async updateOwner(id, dto) {
    await api.put(`/VehicleOwners/${id}`, dto);
    this.fetchOwners();
  }

  async deleteOwner(id) {
    await api.delete(`/VehicleOwners/${id}`);

    const newTotal = this.totalCount - 1;
    const newPages = Math.max(Math.ceil(newTotal / this.pageSize), 1);

    if (this.pageNumber > newPages) {
      this.pageNumber = newPages;
    }

    await this.fetchOwners();
  }
}

export default new VehicleOwnerStore();
