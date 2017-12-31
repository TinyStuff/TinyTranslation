import Vue from 'vue'
import axios from 'axios'
import VueRouter from 'vue-router'
import HomePage from './components/home-page'
import Translations from './components/translations'

export const routes = [
    { path: '/', component: HomePage, display: 'Home', style: 'glyphicon glyphicon-home' },
    { path: '/translate', component: Translations, display: 'Translate', style: 'glyphicon glyphicon-home' }
]

Vue.use(VueRouter);
Vue.prototype.$http = axios;

let router = new VueRouter({
    mode: 'history',
    routes
});

let v = new Vue({
    el: "#app",
    router: router
});