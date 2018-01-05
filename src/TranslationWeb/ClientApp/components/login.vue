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
    .valuetd {
        background-color:#fcfcfc;
        border-bottom: 1px #aaa;
    }
    .right {
        float:right;
        font-size:20px;
    }
    .langs {
        list-style:none;
        margin:0;
        padding:0;
    }

    .keyvalue {
        text-align:right;
    }
    .tools {
        text-align:right;
    }
    .tools > span {
        margin-left: 10px;
        text-align: center;
        height:20px;
        width:20px;
        display:inline-block;
        line-height:20px;
        font-size: 12px;
        color: #fff;
        border-radius: 20px;
        background-color: red;
    }

    .langs li {
        display:inline-block;
        margin-right: 10px;
        padding: 2px 8px;
        text-transform: uppercase;
        color: #fff;
        border-radius: 20px;
        background-color: #53B7CE;
    }

    .value input {
        width: 190px;
        display:inline-block;
    }
</style>