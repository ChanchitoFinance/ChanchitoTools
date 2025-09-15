// Load Firebase configuration from firebase-config.json
let firebaseConfig = null;

// Load Firebase config from JSON file
async function loadFirebaseConfig() {
    try {
        const response = await fetch('./firebase-config.json');
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        firebaseConfig = await response.json();
        
        console.log('Firebase config loaded successfully:', firebaseConfig);
        console.log(`Project ID: ${firebaseConfig.projectId}`);
        
        return firebaseConfig;
    } catch (error) {
        console.error('Error loading Firebase config:', error);
        throw error;
    }
}

// Import Firebase modules
import { initializeApp } from 'https://www.gstatic.com/firebasejs/10.7.1/firebase-app.js';
import { 
    getAuth, 
    signInWithPopup, 
    GoogleAuthProvider, 
    signInAnonymously,
    signInWithEmailAndPassword,
    createUserWithEmailAndPassword,
    onAuthStateChanged,
    signOut
} from 'https://www.gstatic.com/firebasejs/10.7.1/firebase-auth.js';

let app = null;
let auth = null;
let googleProvider = null;

async function initializeFirebase() {
    const config = await loadFirebaseConfig();
    app = initializeApp(config);
    auth = getAuth(app);
    googleProvider = new GoogleAuthProvider();
    
    // Set up auth state listener after Firebase is initialized
    authStateListener = onAuthStateChanged(auth, (user) => {
        if (user) {
            console.log('User signed in:', user);
        } else {
            console.log('User signed out');
        }
    });
    
    console.log('Firebase initialized successfully');
}

// DOM elements
const elements = {
    googleSignIn: document.getElementById('googleSignIn'),
    googleLoading: document.getElementById('googleLoading'),
    googleStatus: document.getElementById('googleStatus'),
    googleUserInfo: document.getElementById('googleUserInfo'),
    googleToken: document.getElementById('googleToken'),
    
    anonymousSignIn: document.getElementById('anonymousSignIn'),
    anonymousLoading: document.getElementById('anonymousLoading'),
    anonymousStatus: document.getElementById('anonymousStatus'),
    anonymousUserInfo: document.getElementById('anonymousUserInfo'),
    anonymousToken: document.getElementById('anonymousToken'),
    
    // Email/Password elements
    emailInput: document.getElementById('emailInput'),
    passwordInput: document.getElementById('passwordInput'),
    signInEmail: document.getElementById('signInEmail'),
    signInLoading: document.getElementById('signInLoading'),
    createAccount: document.getElementById('createAccount'),
    createLoading: document.getElementById('createLoading'),
    createStatus: document.getElementById('createStatus'),
    createUserInfo: document.getElementById('createUserInfo'),
    createToken: document.getElementById('createToken'),
    
    clearAll: document.getElementById('clearAll')
};

// Utility functions
function showLoading(loadingElement) {
    loadingElement.classList.remove('hidden');
}

function hideLoading(loadingElement) {
    loadingElement.classList.add('hidden');
}

function showStatus(statusElement, message, type = 'info') {
    statusElement.textContent = message;
    statusElement.className = `status ${type}`;
    statusElement.classList.remove('hidden');
}

function hideStatus(statusElement) {
    statusElement.classList.add('hidden');
}

function showUserInfo(userInfoElement, user) {
    const userInfo = `
        <strong>User ID:</strong> ${user.uid}<br>
        <strong>Email:</strong> ${user.email || 'N/A'}<br>
        <strong>Display Name:</strong> ${user.displayName || 'N/A'}<br>
        <strong>Provider:</strong> ${user.providerData[0]?.providerId || 'Anonymous'}<br>
        <strong>Email Verified:</strong> ${user.emailVerified ? 'Yes' : 'No'}
    `;
    userInfoElement.innerHTML = userInfo;
    userInfoElement.classList.remove('hidden');
}

function showToken(tokenElement, token) {
    tokenElement.textContent = token;
    tokenElement.classList.remove('hidden');
}

function clearSection(prefix) {
    const elements = [
        `${prefix}Status`,
        `${prefix}UserInfo`, 
        `${prefix}Token`
    ];
    
    elements.forEach(elementId => {
        const element = document.getElementById(elementId);
        if (element) {
            element.classList.add('hidden');
            if (elementId.includes('Token')) {
                element.textContent = '';
            } else if (elementId.includes('UserInfo')) {
                element.innerHTML = '';
            }
        }
    });
}

// Google Sign In
async function signInWithGoogle() {
    if (!auth || !googleProvider) {
        showStatus(elements.googleStatus, 'Firebase not initialized yet. Please wait...', 'error');
        return;
    }
    
    try {
        showLoading(elements.googleLoading);
        hideStatus(elements.googleStatus);
        
        const result = await signInWithPopup(auth, googleProvider);
        const user = result.user;
        
        // Get the ID token
        const idToken = await user.getIdToken();
        
        showStatus(elements.googleStatus, 'Successfully signed in with Google!', 'success');
        showUserInfo(elements.googleUserInfo, user);
        showToken(elements.googleToken, idToken);
        
    } catch (error) {
        console.error('Google sign-in error:', error);
        showStatus(elements.googleStatus, `Error: ${error.message}`, 'error');
    } finally {
        hideLoading(elements.googleLoading);
    }
}

// Anonymous Sign In
async function signInAnonymouslyUser() {
    if (!auth) {
        showStatus(elements.anonymousStatus, 'Firebase not initialized yet. Please wait...', 'error');
        return;
    }
    
    try {
        showLoading(elements.anonymousLoading);
        hideStatus(elements.anonymousStatus);
        
        const result = await signInAnonymously(auth);
        const user = result.user;
        
        // Get the ID token
        const idToken = await user.getIdToken();
        
        showStatus(elements.anonymousStatus, 'Successfully signed in anonymously!', 'success');
        showUserInfo(elements.anonymousUserInfo, user);
        showToken(elements.anonymousToken, idToken);
        
    } catch (error) {
        console.error('Anonymous sign-in error:', error);
        showStatus(elements.anonymousStatus, `Error: ${error.message}`, 'error');
    } finally {
        hideLoading(elements.anonymousLoading);
    }
}

// Sign in with Email/Password
async function signInWithEmailPassword() {
    if (!auth) {
        showStatus(elements.createStatus, 'Firebase not initialized yet. Please wait...', 'error');
        return;
    }
    
    const email = elements.emailInput.value.trim();
    const password = elements.passwordInput.value.trim();
    
    if (!email || !password) {
        showStatus(elements.createStatus, 'Please enter both email and password', 'error');
        return;
    }
    
    try {
        showLoading(elements.signInLoading);
        hideStatus(elements.createStatus);
        
        const result = await signInWithEmailAndPassword(auth, email, password);
        const user = result.user;
        const idToken = await user.getIdToken();
        
        showStatus(elements.createStatus, 'Successfully signed in with email/password!', 'success');
        showUserInfo(elements.createUserInfo, user);
        showToken(elements.createToken, idToken);
        
    } catch (error) {
        console.error('Email/password sign-in error:', error);
        
        let errorMessage = error.message;
        if (error.code === 'auth/user-not-found') {
            errorMessage = 'No user found with this email. Please create an account first.';
        } else if (error.code === 'auth/wrong-password') {
            errorMessage = 'Incorrect password. Please try again.';
        } else if (error.code === 'auth/invalid-email') {
            errorMessage = 'Invalid email address. Please check your email format.';
        } else if (error.code === 'auth/operation-not-allowed') {
            errorMessage = 'Email/password authentication is not enabled. Please enable it in Firebase Console.';
        }
        
        showStatus(elements.createStatus, `Error: ${errorMessage}`, 'error');
    } finally {
        hideLoading(elements.signInLoading);
    }
}

// Create account with Email/Password
async function createAccountWithEmailPassword() {
    if (!auth) {
        showStatus(elements.createStatus, 'Firebase not initialized yet. Please wait...', 'error');
        return;
    }
    
    const email = elements.emailInput.value.trim();
    const password = elements.passwordInput.value.trim();
    
    if (!email || !password) {
        showStatus(elements.createStatus, 'Please enter both email and password', 'error');
        return;
    }
    
    if (password.length < 6) {
        showStatus(elements.createStatus, 'Password must be at least 6 characters long', 'error');
        return;
    }
    
    try {
        showLoading(elements.createLoading);
        hideStatus(elements.createStatus);
        
        const result = await createUserWithEmailAndPassword(auth, email, password);
        const user = result.user;
        const idToken = await user.getIdToken();
        
        showStatus(elements.createStatus, 'Account created successfully! You are now signed in.', 'success');
        showUserInfo(elements.createUserInfo, user);
        showToken(elements.createToken, idToken);
        
    } catch (error) {
        console.error('Account creation error:', error);
        
        let errorMessage = error.message;
        if (error.code === 'auth/email-already-in-use') {
            errorMessage = 'An account with this email already exists. Please sign in instead.';
        } else if (error.code === 'auth/invalid-email') {
            errorMessage = 'Invalid email address. Please check your email format.';
        } else if (error.code === 'auth/weak-password') {
            errorMessage = 'Password is too weak. Please choose a stronger password.';
        } else if (error.code === 'auth/operation-not-allowed') {
            errorMessage = 'Email/password authentication is not enabled. Please enable it in Firebase Console.';
        }
        
        showStatus(elements.createStatus, `Error: ${errorMessage}`, 'error');
    } finally {
        hideLoading(elements.createLoading);
    }
}

// Clear all sections
function clearAllSections() {
    clearSection('google');
    clearSection('anonymous');
    clearSection('create');
    
    // Clear form inputs
    elements.emailInput.value = '';
    elements.passwordInput.value = '';
    
    // Also sign out from Firebase if it's initialized
    if (auth) {
        signOut(auth).catch(console.error);
    }
}

// Event listeners
elements.googleSignIn.addEventListener('click', signInWithGoogle);
elements.anonymousSignIn.addEventListener('click', signInAnonymouslyUser);
elements.signInEmail.addEventListener('click', signInWithEmailPassword);
elements.createAccount.addEventListener('click', createAccountWithEmailPassword);
elements.clearAll.addEventListener('click', clearAllSections);

// Allow Enter key to trigger sign in
elements.passwordInput.addEventListener('keypress', (e) => {
    if (e.key === 'Enter') {
        signInWithEmailPassword();
    }
});

// Auth state listener (will be set up after Firebase initialization)
let authStateListener = null;

// Initialize the app when DOM is loaded
document.addEventListener('DOMContentLoaded', async () => {
    console.log('🚀 Initializing Firebase Web Client...');
    
    try {
        await initializeFirebase();
        console.log('✅ Firebase client initialized and ready!');
        
        // Hide loading status
        const loadingStatus = document.getElementById('loadingStatus');
        if (loadingStatus) {
            loadingStatus.classList.add('hidden');
        }
    } catch (error) {
        console.error('❌ Failed to initialize Firebase:', error);
        
        // Show error status
        const loadingStatus = document.getElementById('loadingStatus');
        if (loadingStatus) {
            loadingStatus.innerHTML = '❌ Failed to load Firebase configuration. Check console for details.';
            loadingStatus.className = 'status error';
        }
    }
});
