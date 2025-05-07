import { makeAutoObservable, runInAction, autorun } from 'mobx';
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

    const saved = localStorage.getItem('vehicleMakeStore');
    if (saved) {
      const data = JSON.parse(saved);
      this.makes = data.makes || [];
      this.totalCount = data.totalCount || 0;
      this.pageNumber = data.pageNumber || 1;
      this.pageSize = data.pageSize || 10;
      this.filter = data.filter || '';
      this.sortBy = data.sortBy || '';
      this.sortDescending = data.sortDescending || false;
    }

    autorun(() => {
      localStorage.setItem(
        'vehicleMakeStore',
        JSON.stringify({
          makes: this.makes,
          totalCount: this.totalCount,
          pageNumber: this.pageNumber,
          pageSize: this.pageSize,
          filter: this.filter,
          sortBy: this.sortBy,
          sortDescending: this.sortDescending,
        }),
      );
    });
  }

  get totalPages() {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  get sortedMakes() {
    const list = [...this.makes];
    if (!this.sortBy) return list;
    list.sort((a, b) => {
      const aVal = String(a[this.sortBy]).toLowerCase();
      const bVal = String(b[this.sortBy]).toLowerCase();
      if (aVal < bVal) return this.sortDescending ? 1 : -1;
      if (aVal > bVal) return this.sortDescending ? -1 : 1;
      return 0;
    });
    return list;
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

  setClientSort(field) {
    if (this.sortBy === field) {
      this.sortDescending = !this.sortDescending;
    } else {
      this.sortBy = field;
      this.sortDescending = false;
    }
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
