import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'
import VueRouter from 'vue-router'
import HomePage from './components/home-page'
import Translations from './components/translations'
import Login from './components/login'


export const routes = [
    { path: '/', component: HomePage, display: 'Home', style: 'glyphicon glyphicon-home' },
    { path: '/login', component: Login, display: 'Login', style: 'glyphicon glyphicon-home' },
    { path: '/translate', component: Translations, display: 'Translate', style: 'glyphicon glyphicon-home' }
]

Vue.use(VueRouter);
Vue.use(Vuex);

Vue.prototype.$http = axios;
Vue.prototype.signedIn = false;

let router = new VueRouter({
    mode: 'history',
    routes
});

window.$languageNames = {
    'se': 'Swedish',
    'sv': 'Swedish',
    'no': 'Norwegian',
    'en': 'English',
    'dk': 'Danish',
    'es': 'Spanish'
};

let v = new Vue({
    el: "#app",
    data: {
        token:''
    },
    router: router,
    async created() {
        try {

            this.token = localStorage.getItem('token');
            console.log(this.token);
            if (this.token) {
                this.signedIn = true;
                window.$token = this.token;
                axios.defaults.headers.common['Authorization'] = 'Bearer '+window.$token;
            }
        } catch (error) {
            console.log(error)
        }
    }
});