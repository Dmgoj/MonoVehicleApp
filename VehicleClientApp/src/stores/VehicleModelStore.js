import { makeAutoObservable, runInAction } from 'mobx';
import api from '../common/services/api';
import vehicleMakeStore from './VehicleMakeStore';

class VehicleModelStore {
  models = [];
  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  filter = '';
  makeFilter = '';
  sortBy = '';
  sortDescending = false;
  loading = false;
  error = null;

  currentModel = { id: 0, name: '', abrv: '', vehicleMakeId: '' };

  constructor() {
    makeAutoObservable(this);
    const saved = localStorage.getItem('vehicleModelStore');
    if (saved) {
      const data = JSON.parse(saved);
      this.models = data.models || [];
      this.totalCount = data.totalCount || 0;
      this.pageNumber = data.pageNumber || 1;
      this.pageSize = data.pageSize || 10;
      this.filter = data.filter || '';
      this.makeFilter = data.makeFilter || '';
      this.sortBy = data.sortBy || '';
      this.sortDescending = data.sortDescending || false;
    }
  }

  get totalPages() {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  get sortedModels() {
    const list = [...this.models];
    if (!this.sortBy) return list;
    if (this.sortBy === 'vehicleMakeName') {
      // ← special case
      list.sort((a, b) => {
        const aName = vehicleMakeStore.makes.find(m => m.id === a.vehicleMakeId)?.name?.toLowerCase() || '';
        const bName = vehicleMakeStore.makes.find(m => m.id === b.vehicleMakeId)?.name?.toLowerCase() || '';
        if (aName < bName) return this.sortDescending ? 1 : -1;
        if (aName > bName) return this.sortDescending ? -1 : 1;
        return 0;
      });
      return list;
    }
    list.sort((a, b) => {
      const aVal = String(a[this.sortBy]).toLowerCase();
      const bVal = String(b[this.sortBy]).toLowerCase();
      if (aVal < bVal) return this.sortDescending ? 1 : -1;
      if (aVal > bVal) return this.sortDescending ? -1 : 1;
      return 0;
    });
    return list;
  }

  async fetchModels() {
    this.loading = true;
    this.error = null;
    try {
      const params = { pageNumber: this.pageNumber, pageSize: this.pageSize };
      if (this.filter) params.filter = this.filter;
      if (this.makeFilter) params.makeId = this.makeFilter;
      const { data } = await api.get('/VehicleModels', { params });
      runInAction(() => {
        this.models = data.items;
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

  setMakeFilter(id) {
    this.makeFilter = id;
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
    if (this.pageNumber > this.totalPages) {
      runInAction(() => {
        this.pageNumber = Math.max(this.totalPages, 1);
      });
      this.fetchModels();
    }
  }

  initNew() {
    this.currentModel = { id: 0, name: '', abrv: '', vehicleMakeId: '' };
  }

  async loadModelForEdit(id) {
    this.loading = true;
    try {
      const { data } = await api.get(`/VehicleModels/${id}`);
      runInAction(() => {
        this.currentModel = {
          id: data.id,
          name: data.name,
          abrv: data.abrv,
          vehicleMakeId: data.vehicleMakeId.toString(),
        };
      });
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  }

  setEditName(v) {
    this.currentModel.name = v;
  }

  setEditAbrv(v) {
    this.currentModel.abrv = v;
  }

  setEditMakeId(v) {
    this.currentModel.vehicleMakeId = v;
  }

  async saveEdit() {
    const dto = {
      name: this.currentModel.name.trim(),
      abrv: this.currentModel.abrv.trim(),
      vehicleMakeId: Number(this.currentModel.vehicleMakeId),
    };
    if (this.currentModel.id) {
      await this.updateModel(this.currentModel.id, dto);
    } else {
      await this.createModel(dto);
    }
  }
}

export default new VehicleModelStore();
