<template>
    <div>
        <h2>Login</h2>
        <div v-if="hasToken">
            Logged in
        </div>
        <div class="loginbox">
            
            <label for="username">Username</label>
            <input id="username" v-model="username" />
            <label for="password">Password</label>
            <input id="password" v-model="password" type="password" />
            <button class="button" @click="signin">Sign in</button>
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
    .loginbox {
        width:300px;
        margin:0 auto;
        padding:40px;
        background:#fff;
        border-radius:10px;
        border: 1px solid rgba(0, 0, 0, 0.3);
        -webkit-box-shadow: 0 3px 7px rgba(0, 0, 0, 0.3);
        -moz-box-shadow: 0 3px 7px rgba(0, 0, 0, 0.3);
        box-shadow: 0 3px 7px rgba(0, 0, 0, 0.3);
        -webkit-background-clip: padding-box;
        -moz-background-clip: padding-box;
        background-clip: padding-box;
    }
</style>