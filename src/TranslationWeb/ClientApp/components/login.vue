<template>
    <div>
        <h2>Login</h2>
        <div v-if="hasToken">
            Logged in
        </div>
        <div v-if="!hasToken">
            Logged in
            <label for="username">Username</label>
            <input id="username" v-model="username" />
            <label for="password">Username</label>
            <input id="password" v-model="password" type="password" />
            <div class="button" @click="signin">Sign in</div>
        </div>
    </div>
</template>
<script>
import Vue from 'vue'

export default {
    data() {
        return {
            hasToken:false,
            username:'',
            password:'',
            token: ''
        };
    },
    methods: {
        signin:function() {
            var url = '/api/admin/login/'+this.username+'?password='+this.password;
            this.$http.get(url).then(response => {
                console.log(response);
                if (response.status==200) {
                    this.hasToken = true;
                    this.token = window.$token = response.data;
                    localStorage.setItem('token',response.data);
                }
            });
        }
    },
    computed: {

    },
    async created() {
        try {

            this.token = localStorage.getItem('token');
            console.log(this.token);
            if (this.token) {
                this.hasToken = true;
                window.$token = this.token;
            }
        } catch (error) {
            console.log(error)
        }
    }
}
</script>
<style>
    
</style>