import { useEffect, useState } from "react";
import { memberService } from "../services/memberService";
import { membershipService } from "../services/membershipService";
import { Member, MemberUpdate } from "../interfaces";
import { User, Mail, CreditCard, ShieldCheck, Edit3, X, Save, LogOut, ArrowRight, ShieldAlert, Loader2 } from "lucide-react";
import axios from "axios";
import "./Profile.css";

export const Profile = () => {
  const [member, setMember] = useState<Member | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [isSaving, setIsSaving] = useState<boolean>(false);
  const [editForm, setEditForm] = useState<MemberUpdate>({
    memberName: "",
    memberSurname: "",
    memberEmail: "",
    memberPhone: "",
  });

  const fetchProfile = async () => {
    try {
      setLoading(true);
      const data = await memberService.getMyInfo();
      setMember(data);
      setEditForm({
        memberName: data.memberName,
        memberSurname: data.memberSurname,
        memberEmail: data.memberEmail,
        memberPhone: data.memberPhone,
      });
      setError(null);
    } catch {
      setError("Failed to load profile data. Please try logging in again.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProfile();
  }, []);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEditForm({ ...editForm, [e.target.name]: e.target.value });
  };

  const handleSave = async () => {
    if (!member) return;
    setIsSaving(true);
    try {
      await memberService.updateMember(member.memberId, editForm);
      await fetchProfile();
      setIsEditing(false);
    } catch (err) {
      alert("Failed to update profile. Please ensure your email is unique and data is valid.");
    } finally {
      setIsSaving(false);
    }
  };

  const handleCancel = () => {
    if (member) {
      setEditForm({
        memberName: member.memberName,
        memberSurname: member.memberSurname,
        memberEmail: member.memberEmail,
        memberPhone: member.memberPhone,
      });
    }
    setIsEditing(false);
  };

  const handleCancelMembership = async () => {
    if (!member || !member.membershipId) return;

    if (!window.confirm("Are you sure you want to cancel your membership? This will end your access immediately.")) {
      return;
    }

    setLoading(true);
    try {
      await membershipService.cancelMembership(member.membershipId);
      alert("Membership successfully cancelled.");
      await fetchProfile();
    } catch (err) {
      if (axios.isAxiosError(err)) {
        alert(err.response?.data || "Failed to cancel membership.");
      } else {
        alert("An unexpected error occurred.");
      }
    } finally {
      setLoading(false);
    }
  };

  if (loading && !member) return (
    <div className="container" style={{ minHeight: '60vh', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
      <div className="text-center">
        <Loader2 className="spinner text-primary" size={40} style={{ marginBottom: '1rem' }} />
        <div className="form-subtitle animate-pulse">Syncing Profile...</div>
      </div>
    </div>
  );

  if (error) return (
    <div className="container" style={{ paddingTop: '5rem' }}>
      <div className="glass-card text-center" style={{ padding: '3rem', maxWidth: '500px', margin: '0 auto', borderColor: 'var(--error)' }}>
        <ShieldAlert className="text-error" size={48} style={{ marginBottom: '1rem' }} />
        <h2 className="uppercase font-black tracking-tight" style={{ fontSize: '1.5rem', marginBottom: '0.5rem' }}>Sync Error</h2>
        <p className="text-muted font-medium">{error}</p>
      </div>
    </div>
  );

  if (!member) return <div className="container text-center text-dim uppercase font-black" style={{ paddingTop: '5rem' }}>No profile found.</div>;

  const isActive = member.membership && new Date(member.membership.membershipTo) > new Date();
  const initials = `${member.memberName[0]}${member.memberSurname[0]}`.toUpperCase();

  return (
    <div className="profile-container container">
      {/* Background Decor */}
      <div style={{ position: 'fixed', top: 0, left: 0, width: '100%', height: '300px', background: 'linear-gradient(to b, rgba(59, 130, 246, 0.1), transparent)', pointerEvents: 'none', zIndex: -1 }} />
      
      {/* Profile Header */}
      <div className="profile-header animate-fade-in">
        <div className="profile-avatar">
          {initials}
        </div>
        <div className="profile-info">
          <div className="profile-name-row">
            <h1 className="profile-name font-black uppercase tracking-tighter">{member.memberName} {member.memberSurname}</h1>
            {isActive && <span className="badge-active">Pro Member</span>}
          </div>
          <p className="profile-email">
            <Mail size={16} className="text-primary" /> {member.memberEmail}
          </p>
        </div>
        {!isEditing && (
          <button 
            onClick={() => setIsEditing(true)}
            className="btn-outline"
          >
            <Edit3 size={16} /> Edit Profile
          </button>
        )}
      </div>

      <div className="profile-grid">
        
        {/* Details & Membership */}
        <div className="animate-fade-in delay-100" style={{ display: 'flex', flexDirection: 'column', gap: '2rem' }}>
          
          {/* Personal Details Card */}
          <div className="glass-card" style={{ padding: '2.5rem', position: 'relative', overflow: 'hidden' }}>
            <div className="form-header-glow" />
            <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: '2rem' }}>
              <h2 className="uppercase font-black tracking-tight" style={{ fontSize: '1.25rem', display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
                <User className="text-primary" /> Account Details
              </h2>
            </div>

            {isEditing ? (
              <div className="animate-fade-in" style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
                <div className="detail-group">
                  <div className="form-group">
                    <label className="detail-label">First Name</label>
                    <input type="text" name="memberName" value={editForm.memberName} onChange={handleInputChange} className="input-field" />
                  </div>
                  <div className="form-group">
                    <label className="detail-label">Last Name</label>
                    <input type="text" name="memberSurname" value={editForm.memberSurname} onChange={handleInputChange} className="input-field" />
                  </div>
                </div>
                <div className="form-group">
                  <label className="detail-label">Email Address</label>
                  <input type="email" name="memberEmail" value={editForm.memberEmail} onChange={handleInputChange} className="input-field" />
                </div>
                <div className="form-group">
                  <label className="detail-label">Phone Number</label>
                  <input type="tel" name="memberPhone" value={editForm.memberPhone} onChange={handleInputChange} className="input-field" />
                </div>

                <div style={{ display: 'flex', gap: '1rem', paddingTop: '1rem' }}>
                  <button onClick={handleSave} disabled={isSaving} className="btn-premium" style={{ flex: 1 }}>
                    {isSaving ? <Loader2 className="spinner" size={18} /> : <Save size={18} />} {isSaving ? "Saving..." : "Update Details"}
                  </button>
                  <button onClick={handleCancel} disabled={isSaving} className="btn-outline" style={{ padding: '0 1.5rem' }}>
                    <X size={18} />
                  </button>
                </div>
              </div>
            ) : (
              <div className="detail-group">
                <div className="detail-item">
                  <span className="detail-label">Legal Name</span>
                  <span className="detail-value">{member.memberName} {member.memberSurname}</span>
                </div>
                <div className="detail-item">
                  <span className="detail-label">Secure Email</span>
                  <span className="detail-value">{member.memberEmail}</span>
                </div>
                <div className="detail-item" style={{ gridColumn: 'span 2' }}>
                  <span className="detail-label">Mobile Contact</span>
                  <span className="detail-value">{member.memberPhone || "Not established"}</span>
                </div>
              </div>
            )}
          </div>

          {/* Support/Quick Actions Card */}
          <div className="detail-group">
             <button className="glass-card action-card group transition-all">
               <div className="action-icon"><ShieldCheck size={24} /></div>
               <div className="action-content">
                  <div className="action-title">Security</div>
                  <div className="action-desc">Update Password</div>
               </div>
               <ArrowRight size={16} className="text-dim transition-all" />
             </button>
             <button className="glass-card action-card group transition-all">
               <div className="action-icon" style={{ color: 'var(--error)', background: 'rgba(239, 68, 68, 0.1)' }}><LogOut size={24} /></div>
               <div className="action-content">
                  <div className="action-title">Session</div>
                  <div className="action-desc">Logout Account</div>
               </div>
               <ArrowRight size={16} className="text-dim transition-all" />
             </button>
          </div>
        </div>

        {/* Sidebar Membership Status */}
        <div className="animate-fade-in delay-200">
          <div className="glass-card status-card" style={{ padding: '2rem', position: 'relative', overflow: 'hidden' }}>
            <div style={{ position: 'absolute', top: 0, left: 0, width: '100%', height: '4px', background: isActive ? 'var(--success)' : 'var(--error)' }} />
            <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem', marginBottom: '2rem' }}>
               <CreditCard className={isActive ? 'text-success' : 'text-error'} />
               <h2 className="uppercase font-black tracking-tight" style={{ fontSize: '1.25rem' }}>Subscription</h2>
            </div>
            
            {member.membership && isActive ? (
              <div style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
                <div className="status-badge-large">
                  <span className="status-label">Current Tier</span>
                  <span className="status-value">{member.membership.membershipPlan?.planName || "Active"}</span>
                </div>
                
                <div style={{ display: 'flex', flexDirection: 'column' }}>
                  <div className="info-row">
                    <span className="info-label">Started</span>
                    <span className="info-value">{new Date(member.membership.membershipFrom).toLocaleDateString()}</span>
                  </div>
                  <div className="info-row">
                    <span className="info-label">Expires</span>
                    <span className="info-value" style={{ borderBottom: '2px solid var(--primary)' }}>{new Date(member.membership.membershipTo).toLocaleDateString()}</span>
                  </div>
                  <div className="info-row">
                    <span className="info-label">Payment</span>
                    {member.membership.isFeePaid ? (
                      <span className="badge-active">Verified</span>
                    ) : (
                      <span className="badge-pending">Unpaid</span>
                    )}
                  </div>
                  <div className="info-row">
                    <span className="info-label">Investment</span>
                    <span className="info-value" style={{ fontSize: '1.25rem', fontWeight: 900 }}>${member.membership.membershipFee.toFixed(2)}</span>
                  </div>
                </div>

                <button 
                  onClick={handleCancelMembership}
                  className="text-error font-black uppercase tracking-widest"
                  style={{ fontSize: '0.625rem', background: 'none', border: 'none', padding: '1rem', cursor: 'pointer', opacity: 0.6, display: 'flex', alignItems: 'center', justifyContent: 'center', gap: '0.5rem' }}
                >
                  <X size={14} /> Terminate Subscription
                </button>
              </div>
            ) : (
              <div className="text-center" style={{ padding: '2rem 0' }}>
                <div style={{ width: '5rem', height: '5rem', background: 'rgba(239, 68, 68, 0.1)', borderRadius: '1.5rem', display: 'flex', alignItems: 'center', justifyContent: 'center', margin: '0 auto 1.5rem', border: '1px solid rgba(239, 68, 68, 0.2)' }}>
                  <CreditCard className="text-error" size={40} />
                </div>
                <h3 className="uppercase font-black tracking-tight" style={{ fontSize: '1.25rem', marginBottom: '0.5rem' }}>No Active Access</h3>
                <p className="text-muted font-medium" style={{ fontSize: '0.875rem', marginBottom: '2rem' }}>Establish your membership today and unlock full training facilities.</p>
                <a href="/plans" className="btn-premium" style={{ width: '100%' }}>
                  View Tiers <ArrowRight size={18} />
                </a>
              </div>
            )}
          </div>
        </div>

      </div>
    </div>
  );
};
