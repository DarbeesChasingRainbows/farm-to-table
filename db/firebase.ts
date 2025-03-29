import { initializeApp } from "firebase/app";
import { getFirestore, collection, doc, setDoc, getDoc, getDocs, query, where } from "firebase/firestore";

// Firebase configuration from environment variables
const firebaseConfig = {
  apiKey: Deno.env.get("FIREBASE_API_KEY"),
  authDomain: Deno.env.get("FIREBASE_AUTH_DOMAIN"),
  projectId: Deno.env.get("FIREBASE_PROJECT_ID"),
  storageBucket: Deno.env.get("FIREBASE_STORAGE_BUCKET"),
  messagingSenderId: Deno.env.get("FIREBASE_MESSAGING_SENDER_ID"),
  appId: Deno.env.get("FIREBASE_APP_ID"),
};

// Check if Firebase environment variables are set
const isFirebaseConfigured = Object.values(firebaseConfig).every(Boolean);

// Initialize Firebase app and Firestore
let app;
let db;

if (isFirebaseConfigured) {
  try {
    app = initializeApp(firebaseConfig);
    db = getFirestore(app);
    console.log("Firebase initialized successfully as backup database");
  } catch (error) {
    console.error("Error initializing Firebase:", error);
  }
} else {
  console.warn("Firebase configuration incomplete. Backup database not initialized.");
}

// Helper functions for Firebase operations
export async function saveToFirebase(collectionName: string, id: string, data: Record<string, unknown>) {
  if (!db) {
    console.warn("Firebase not initialized. Data not saved to backup.");
    return null;
  }
  
  try {
    const docRef = doc(db, collectionName, id);
    await setDoc(docRef, { ...data, updatedAt: new Date() });
    return id;
  } catch (error) {
    console.error(`Error saving to Firebase collection ${collectionName}:`, error);
    return null;
  }
}

export async function getFromFirebase(collectionName: string, id: string) {
  if (!db) {
    console.warn("Firebase not initialized. Cannot retrieve data from backup.");
    return null;
  }
  
  try {
    const docRef = doc(db, collectionName, id);
    const docSnap = await getDoc(docRef);
    
    if (docSnap.exists()) {
      return docSnap.data();
    } else {
      return null;
    }
  } catch (error) {
    console.error(`Error getting document from Firebase collection ${collectionName}:`, error);
    return null;
  }
}

export async function queryFirebase(collectionName: string, field: string, operator: any, value: any) {
  if (!db) {
    console.warn("Firebase not initialized. Cannot query backup database.");
    return [];
  }
  
  try {
    const q = query(collection(db, collectionName), where(field, operator, value));
    const querySnapshot = await getDocs(q);
    
    const results: any[] = [];
    querySnapshot.forEach((doc) => {
      results.push({
        id: doc.id,
        ...doc.data()
      });
    });
    
    return results;
  } catch (error) {
    console.error(`Error querying Firebase collection ${collectionName}:`, error);
    return [];
  }
}

// Export the Firebase app and db if initialized
export { app, db };