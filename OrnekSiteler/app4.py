from flask import Flask, render_template, request, redirect, url_for

app = Flask(__name__)


USERNAME = 'suleyman'
PASSWORD = 'suleymansifre'

@app.route('/')
def login():
    return render_template('login.html')

@app.route('/login', methods=['POST'])
def login_post():
    username = request.form['username']
    password = request.form['password']
    
    
    if username == USERNAME and password == PASSWORD:
        return redirect(url_for('callback', username=username))
    else:
        return "Yanlış kullanıcı adı veya şifre!", 401

@app.route('/callback')
def callback():
    username = request.args.get('username')
    return f"Hosgeldin {username} su an site4' tesiniz"

if __name__ == '__main__':
    app.run(port='5005',debug=True)
