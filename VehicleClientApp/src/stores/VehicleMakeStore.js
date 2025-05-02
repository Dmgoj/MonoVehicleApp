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

  constructor() {
    makeAutoObservable(this);
  }

  async fetchMakes() {
    this.loading = true;
    try {
      const params = {
        pageNumber: this.pageNumber,
        pageSize:   this.pageSize
      };
  
      if (this.filter) {
        params.filter = this.filter;
      }
      if (this.sortBy) {
        params.sortBy       = this.sortBy;
        params.sortDescending = this.sortDescending;
      }
  
      const { data } = await api.get('/VehicleMakes', { params });
      runInAction(() => {
        this.makes      = data.items;
        this.totalCount = data.totalCount;
      });
    } finally {
      runInAction(() => { this.loading = false; });
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
    this.fetchMakes();
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

  async createMake(makeDto) {
    const { data } = await api.post('/VehicleMakes', makeDto);
    runInAction(() => {
      this.pageNumber = 1;
      this.fetchMakes();
    });
    return data;
  }

  async updateMake(id, makeDto) {
    await api.put(`/VehicleMakes/${id}`, makeDto);
    // refresh current page
    runInAction(() => {
      this.fetchMakes();
    });
  }

  async deleteMake(id) {
    await api.delete(`/VehicleMakes/${id}`);
    runInAction(() => this.fetchMakes());
  }
}

export default new VehicleMakeStore();