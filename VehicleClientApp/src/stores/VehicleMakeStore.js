import { makeAutoObservable, runInAction } from 'mobx';
import api from '../common/services/api';

class VehicleMakeStore {
  makes = [];
  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  filter = '';
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

  async fetchMakes() {
    this.loading = true;
    this.error = null;
    try {
      const params = {
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
        filter: this.filter || undefined,
        sortBy: this.sortBy || undefined,
        sortDescending: this.sortDescending || undefined,
      };
      const { data } = await api.get('/VehicleMakes', { params });
      runInAction(() => {
        this.makes = data.items;
        this.totalCount = data.totalCount;
      });
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
      console.error('Error loading makes', err);
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  }

  setPage(page) {
    this.pageNumber = page;
    this.fetchMakes();
  }

  setPageSize(size) {
    this.pageSize = size;
    this.pageNumber = 1;
    this.fetchMakes();
  }

  setFilter(text) {
    this.filter = text;
    this.pageNumber = 1;
    if (this._filterDebounce) clearTimeout(this._filterDebounce);
    this._filterDebounce = setTimeout(() => this.fetchMakes(), 300);
  }

  setSort(field) {
    if (this.sortBy === field) {
      this.sortDescending = !this.sortDescending;
    } else {
      this.sortBy = field;
      this.sortDescending = false;
    }
    this.fetchMakes();
  }

  async createMake(dto) {
    const { data } = await api.post('/VehicleMakes', dto);
    runInAction(() => {
      this.pageNumber = 1;
    });
    this.fetchMakes();
    return data;
  }

  async updateMake(id, dto) {
    await api.put(`/VehicleMakes/${id}`, dto);
    this.fetchMakes();
  }

  async deleteMake(id) {
    await api.delete(`/VehicleMakes/${id}`);
    this.fetchMakes();
  }
}

export default new VehicleMakeStore();
