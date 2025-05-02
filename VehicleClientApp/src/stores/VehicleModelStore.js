// src/stores/VehicleModelStore.js
import { makeAutoObservable, runInAction } from 'mobx';
import api from '../common/services/api';

class VehicleModelStore {
  // ── Observables ──────────────────────────────────────────
  models = [];
  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  filter = ''; // text search on model name
  makeFilter = ''; // ID of the make to filter by
  sortBy = '';
  sortDescending = false;
  loading = false;
  error = null;

  constructor() {
    makeAutoObservable(this);
  }

  // ── Computed ─────────────────────────────────────────────
  get totalPages() {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  // ── Data loading ─────────────────────────────────────────
  async fetchModels() {
    this.loading = true;
    this.error = null;

    try {
      const params = {
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
      };
      if (this.filter) params.filter = this.filter;
      if (this.makeFilter) params.makeId = this.makeFilter; // passes makeId to API
      if (this.sortBy) {
        params.sortBy = this.sortBy;
        params.sortDescending = this.sortDescending;
      }

      const { data } = await api.get('/VehicleModels', { params });

      runInAction(() => {
        this.models = data.items;
        this.totalCount = data.totalCount;
      });
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
      console.error('Error loading models', err);
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  }

  // ── Actions ───────────────────────────────────────────────
  setPage(page) {
    this.pageNumber = page;
    this.fetchModels();
  }

  setPageSize(size) {
    this.pageSize = size;
    this.pageNumber = 1;
    this.fetchModels();
  }

  setFilter(text) {
    this.filter = text;
    this.pageNumber = 1;
    this.fetchModels();
  }

  setMakeFilter(makeId) {
    this.makeFilter = makeId;
    this.pageNumber = 1;
    this.fetchModels();
  }

  setSort(field) {
    if (this.sortBy === field) {
      this.sortDescending = !this.sortDescending;
    } else {
      this.sortBy = field;
      this.sortDescending = false;
    }
    this.fetchModels();
  }

  async createModel(dto) {
    const { data } = await api.post('/VehicleModels', dto);
    runInAction(() => {
      this.pageNumber = 1;
    });
    this.fetchModels();
    return data;
  }

  async updateModel(id, dto) {
    await api.put(`/VehicleModels/${id}`, dto);
    this.fetchModels();
  }

  async deleteModel(id) {
    await api.delete(`/VehicleModels/${id}`);
    this.fetchModels();
  }
}

export default new VehicleModelStore();
