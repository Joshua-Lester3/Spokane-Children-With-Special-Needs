import Axios from 'axios';

export default defineNuxtPlugin(() => {
  if (process.client) {
    if (
      window.location.hostname === 'localhost'
    ) {
      Axios.defaults.baseURL = 'https://localhost:7222/';
    } else {
      Axios.defaults.baseURL = 'azure-url';
    }
  }
});