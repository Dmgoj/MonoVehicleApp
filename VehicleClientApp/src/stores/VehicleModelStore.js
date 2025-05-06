import { makeAutoObservable, runInAction } from 'mobx';
import api from '../common/services/api';

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

  currentModel = {
    id: 0,
    name: '',
    abrv: '',
    vehicleMakeId: '',
  };

  constructor() {
    makeAutoObservable(this);
  }

  get totalPages() {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  async fetchModels() {
    this.loading = true;
    this.error = null;
    try {
      const params = {
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
      };
      if (this.filter) params.filter = this.filter;
      if (this.makeFilter) params.makeId = this.makeFilter;
      if (this.sortBy) {
        params.sortBy = this.sortBy;
        params.sortDescending = this.sortDescending;
      }
      const { data } = await api.get('/VehicleModels', { params });
      runInAction(() => {
        this.models = data.items;
        this.totalCount = data.totalCount;
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
    await this.fetchModels();
    if (this.pageNumber > this.totalPages) {
      runInAction(() => {
        this.pageNumber = Math.max(this.totalPages, 1);
      });

      await this.fetchModels();
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
